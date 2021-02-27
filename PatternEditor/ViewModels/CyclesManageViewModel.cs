using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Common;
using Common.Helpers;
using Microsoft.Win32;

namespace PatternEditor.ViewModels
{
    public class CyclesManageViewModel : ViewModel
    {
        private int _numDevices = 1;
        private string _savePath = string.Empty;
        private DeviceManagerViewModel _selectedItem;
        private bool _isPreview;

        public ObservableCollection<DeviceManagerViewModel> Cycles { get; set; }

        
        public string PreviewText => "Preview";

        public DeviceManagerViewModel SelectedPreviewItem { get; set; }

        public DeviceManagerViewModel SelectedItem
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

        public bool IsPreview
        {
            get => _isPreview;
            set
            {
                _isPreview = value;
                OnPropertyChanged();
            }
        }

        #region Contructors

        public CyclesManageViewModel()
        {
            _numDevices = 14;
            Cycles = new ObservableCollection<DeviceManagerViewModel>();
            Cycles.Add(new DeviceManagerViewModel(_numDevices) {  CycleNumber = 1});
            SelectedItem = Cycles.First();
            AddCommand = new RelayCommand(OnAddItem);
            DeleteCommand = new RelayCommand(OnDeleteItem);
            CloneCommand = new RelayCommand(OnCloneItem);
            PreviewCommand = new RelayCommand(OnPreview); 
            SaveCommand = new RelayCommand(OnSave, o => !string.IsNullOrEmpty(_savePath));
            SaveAsCommand = new RelayCommand(OnSaveAs);
            OpenCommand = new RelayCommand(OnOpen);
        }

        #endregion

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
                    SelectedPreviewItem = vm;
                    OnPropertyChanged(nameof(SelectedPreviewItem));
                    Thread.Sleep(vm.Delay * 10);
                }
            }
        }

        private void OnAddItem(object obj)
        {
            var item = new DeviceManagerViewModel(_numDevices) {CycleNumber = Cycles .Count+1};
            Cycles.Add(item);
            SelectedItem = item;
        }

        private void OnDeleteItem(object obj)
        {
            var vm = obj as DeviceManagerViewModel;
            if (vm == null)
            {
                return;
            }

            Cycles.Remove(vm);
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
            var vm = obj as DeviceManagerViewModel;
            if (vm == null)
            {
                return;
            }

            var item = new DeviceManagerViewModel(vm) { NumDevices = _numDevices, CycleNumber = Cycles.Count + 1 };
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

            var items = SerializerManager.Deserialize(File.ReadAllText(_savePath));
            foreach (var vm in items)
            {
                vm.PopulateDevices();
            }
            Cycles = new ObservableCollection<DeviceManagerViewModel>(items);
            OnPropertyChanged(nameof(Cycles));
            SelectedItem = Cycles.FirstOrDefault();
        }


        private void SaveInternal()
        {
            var content = this.SerializeToJson();
            File.WriteAllText(_savePath, content);
        }
    }
}