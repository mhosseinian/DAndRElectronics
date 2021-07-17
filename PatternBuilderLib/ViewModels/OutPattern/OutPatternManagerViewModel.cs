using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Common;
using Common.Helpers;
using Common.Services;
using Microsoft.Win32;
using Newtonsoft.Json;
using PatternBuilderLib.Models;

namespace PatternBuilderLib.ViewModels.OutPattern
{
    public interface ICycleManagerView
    {
        bool ShowFlatOvalButton { get; }
        bool IsOutputPattern { get; }
        bool IsPreview { get; set; }
    }
    public class OutPatternManagerViewModel: ViewModel , ICycleManagerView
    {
        private OutPatternModelViewModel _selectedItem;
        private OutPatternModels _models;
        private bool _isPreview;
        public string _savePath = string.Empty;

        #region Contructors

        public OutPatternManagerViewModel(OutPatternModels models)
        {
            _models = models;
            Populate();
            
            AddCommand = new RelayCommand(OnAddItem);
            DeleteCommand = new RelayCommand(OnDeleteItem);
            CloneCommand = new RelayCommand(OnCloneItem);
            PreviewCommand = new RelayCommand(OnPreview);
            SaveCommand = new RelayCommand(OnSave, o => !string.IsNullOrEmpty(_savePath));
            SaveAsCommand = new RelayCommand(OnSaveAs);
            OpenCommand = new RelayCommand(OnOpen);
            
        }

        #endregion


        public IEnumerable<string> KeyNames=> Enumerable.Range(1, 22 ).Select(GetKeyName);

        private static string GetKeyName(int index)
        {
            if (index == 22)
            {
                return "H R L";
            }

            return $"{index}";
        }

      

        [JsonProperty(PropertyName = Constants.JsonOuts)]
        public ObservableCollection<OutPatternModelViewModel> Cycles { get; set; }


        //public string DeleteText => $"Delete Cycle {CycleNumber}";
        //[JsonIgnore] public string CloneText => $"Clone Cycle {CycleNumber}";
        //[JsonIgnore] public string Label => $"Cycle # {CycleNumber}";


        public OutPatternModelViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }
        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand CloneCommand { get; set; }
        public ICommand PreviewCommand { get; set; }
        public ICommand SaveCommand { get; }
        public ICommand SaveAsCommand { get; }
        public ICommand OpenCommand { get; }
        public bool ShowFlatOvalButton { get; set; } = false;//Always false
        public bool IsOutputPattern { get; set; } = true;//Always true

        public bool IsPreview
        {
            get => _isPreview;
            set
            {
                _isPreview = value;
                OnPropertyChanged();
            }
        }

        public string PreviewText => "Preview";
        public OutPatternModelViewModel SelectedPreviewItem { get; set; }

        public int Delay { get; set; } = 50;

        private void OnPreview(object obj)
        {
            IsPreview = true;
            OnPropertyChanged(nameof(PreviewText));
            Task.Factory.StartNew(DoPreview);
        }

        private void DoPreview()
        {
            
            while (IsPreview)
            {
                foreach (var vm in Cycles)
                {
                    SelectedItem = vm;
                    vm.Preview(true);
                    OnPropertyChanged(nameof(SelectedItem));
                    Thread.Sleep(Delay * 10);
                    vm.Preview(false);
                }
            }
            foreach (var outPatternModelViewModel in Cycles)
            {
                outPatternModelViewModel.Preview(false);
            }
        }

        private void OnAddItem(object obj)
        {
            var item = new OutPatternModelViewModel(_models.NewOutPatternModel()) { CycleNumber = Cycles.Count + 1 };
            Cycles.Add(item);
            SelectedItem = item;
        }

        private void OnDeleteItem(object obj)
        {
            var vm = obj as OutPatternModelViewModel;
            if (vm == null)
            {
                return;
            }

            Cycles.Remove(vm);
            _models.Models.Remove(vm.Model);
            if (!Cycles.Any())
            {
                SelectedItem = null;
            }
            else
            {
                SelectedItem = Cycles.Last();
            }
        }

        private void OnCloneItem(object obj)
        {
            var vm = obj as OutPatternModelViewModel;
            if (vm == null)
            {
                return;
            }

            var item = new OutPatternModelViewModel(vm) { CycleNumber = Cycles.Count + 1 };
            _models.Models.Add(item.Model);
            Cycles.Add(item);
            
            SelectedItem = item;

        }

        private void OnSave(object obj)
        {
            SaveInternal();
        }

        private void OnSaveAs(object obj)
        {
            var saveFileDialog = new SaveFileDialog { Filter = "Json files (*.json)|*.json|All files (*.*)|*.*" };
            if (saveFileDialog.ShowDialog() != true)
            {
                return;
            }

            _savePath = saveFileDialog.FileName;
            SaveInternal();
        }

        private void OnOpen(object obj)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Json files (*.json)|*.json|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() != true)
            {
                return;
            }

            _savePath = openFileDialog.FileName;
            OpenInternal();
        }

        public void OpenInternal()
        {
            _models = OutPatternModels.Deserialize(File.ReadAllText(_savePath));
            Populate();
        }

        private void Populate()
        {
            Cycles = new ObservableCollection<OutPatternModelViewModel>(CreateViewModels());
            OnPropertyChanged(nameof(Cycles));
            SelectedItem = Cycles.FirstOrDefault();
        }

        private IEnumerable<OutPatternModelViewModel> CreateViewModels()
        {
            var counter = 1;
            foreach (var outPatternModel in _models.Models)
            {
                yield return new OutPatternModelViewModel(outPatternModel){CycleNumber = counter++};
            }
        }
        

        public void SaveInternal()
        {
            var content = JsonConvert.SerializeObject(_models, Formatting.Indented);
            File.WriteAllText(_savePath, content);
            var binaryFilename = Path.ChangeExtension(_savePath, ".bin");
            SerializeBinaryToFile( binaryFilename);
        }

        private void SerializeBinaryToFile(string filename)
        {
            using var binWriter = new BinaryWriter(File.Open(filename, FileMode.Create));
           _models.Write(binWriter);
        }
    }
}