using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Common;
using Common.Services;
using DAndRElectronics.ButtonViewModels;
using Common.Enums;
using Common.Helpers;
using DAndRElectronics.Services;
using Microsoft.Win32;

namespace DAndRElectronics.View
{
    public class KeyboardViewModel:ViewModel, IButtonService, IEditorEventsSubscriber
    {
        public string _savePath = string.Empty;
        private List<ButtonViewModel> _keyButtons = new List<ButtonViewModel>();
        private List<ButtonViewModel> _inputButtons = new List<ButtonViewModel>();
        private List<ButtonViewModel> _eventButtons = new List<ButtonViewModel>();
        private List<ButtonViewModel> _slideButtons = new List<ButtonViewModel>();
        private List<ButtonViewModel> _analogButtons = new List<ButtonViewModel>();
        private List<ButtonViewModel> _timerButtons = new List<ButtonViewModel>();
        private List<ButtonViewModel> _temperatureButtons = new List<ButtonViewModel>();
        private List<ButtonViewModel> _sensorButtons = new List<ButtonViewModel>();

        public IEnumerable<ButtonViewModel> KeyButtons => _keyButtons;
        public IEnumerable<ButtonViewModel> InputButtons => _inputButtons;
        public IEnumerable<ButtonViewModel> EventButtons => _eventButtons;
        public IEnumerable<ButtonViewModel> SlideButtons => _slideButtons;
        public IEnumerable<ButtonViewModel> AnalogButtons => _analogButtons;
        public IEnumerable<ButtonViewModel> TimerButtons => _timerButtons;
        public IEnumerable<ButtonViewModel> TemperatureButtons => _temperatureButtons;
        public IEnumerable<ButtonViewModel> SensorButtons => _sensorButtons;
        public ICommand SaveCommand { get; }
        public ICommand SaveAsCommand { get; }
        public ICommand OpenCommand { get; }
        public ICommand AddEventCommand { get; }
        

        #region Contructors

        public KeyboardViewModel()
        {
            SaveCommand = new RelayCommand(OnSave, o => !string.IsNullOrEmpty(_savePath));
            SaveAsCommand = new RelayCommand(OnSaveAs);
            OpenCommand = new RelayCommand(OnOpen);
            AddEventCommand = new RelayCommand(OnAddEvent);
            PopulateButtons(_keyButtons, 21, Constants.KeyBaseName);
            PopulateButtons(_inputButtons, 17, Constants.InputBaseName);
            for (var i = 12; i < 16; i++)
            {
                _inputButtons[i].IsEnabled = false;
            }
            //PopulateButtons(_eventButtons, 5, Constants.EventBaseName);
            PopulateButtons(_slideButtons, 3, Constants.SlideBaseName);
            PopulateButtons(_analogButtons, 2, Constants.AnalogBaseName);
            PopulateButtons(_timerButtons, 1, Constants.TimerBaseName);
            PopulateButtons(_temperatureButtons, 1, Constants.TemperatureBaseName);
            PopulateButtons(_sensorButtons, 1, Constants.SensorBaseName);

            var service = ServiceDirectory.Instance.GetService<IStateService>();
            service.SubscribeButtonDelete(this, OnButtonDelete);
            ServiceDirectory.Instance.ButtonService = this;
            ;
            
        }

        private void OnButtonDelete(object obj)
        { 
            //Assuming the event button is being delete
            var vm = obj as EventButtonViewModel;
            if (vm == null)
            {
                return;
            }

            if (_eventButtons.Contains(vm))
            {
                _eventButtons.Remove(vm);
                OnPropertyChanged(nameof(EventButtons));
                var stateService = ServiceDirectory.Instance.GetService<IStateService>();
                stateService.OnStateChanged(StateChangedTypes.EventButtonAdded);
            }
           
        }

        #endregion


        public void OnAddEvent(object obj)
        {
           
            var col = 0;
            var row = 0;
            
            var btnVm = new EventButtonViewModel($"{Constants.EventBaseName}{_eventButtons.Count + 1}", col, row);
            _eventButtons.Add(btnVm);
            SetRowColumnForButtons(_eventButtons);

            OnPropertyChanged(nameof(EventButtons));
            var stateService = ServiceDirectory.Instance.GetService<IStateService>();
            stateService.OnStateChanged(StateChangedTypes.EventButtonAdded);
        }

        private void OnSave(object obj)
        {
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

            OpenJson(openFileDialog.FileName);
        }

        public void OpenJson(string filename)
        {
            _savePath = filename;
            var service = ServiceDirectory.Instance.GetService<IButtonViewModelFactoryService>();
            var allItems = service.ReadFile(_savePath).ToList();


            _keyButtons = allItems.Where(i => i.ButtonName.StartsWith(Constants.KeyBaseName)).ToList();
            _inputButtons = allItems.Where(i => i.ButtonName.StartsWith(Constants.InputBaseName)).ToList();
            _eventButtons = allItems.Where(i => i.ButtonName.StartsWith(Constants.EventBaseName)).ToList();
            _slideButtons = allItems.Where(i => i.ButtonName.StartsWith(Constants.SlideBaseName)).ToList();
            _analogButtons = allItems.Where(i => i.ButtonName.StartsWith(Constants.AnalogBaseName)).ToList();
            _timerButtons = allItems.Where(i => i.ButtonName.StartsWith(Constants.TimerBaseName)).ToList();
            _temperatureButtons = allItems.Where(i => i.ButtonName.StartsWith(Constants.TemperatureBaseName)).ToList();
            _sensorButtons = allItems.Where(i => i.ButtonName.StartsWith(Constants.SensorBaseName)).ToList();

            var stateService = ServiceDirectory.Instance.GetService<IStateService>();
            SetRowColumnForButtons(_keyButtons);
            SetRowColumnForButtons(_inputButtons);
            SetRowColumnForButtons(_eventButtons);
            SetRowColumnForButtons(_slideButtons);
            SetRowColumnForButtons(_analogButtons);
            SetRowColumnForButtons(_timerButtons);
            SetRowColumnForButtons(_eventButtons);

            stateService.OnStateChanged(StateChangedTypes.ProjectOpened);
        }

        private void OnSaveAs(object obj)
        {
            var saveFileDialog = new SaveFileDialog {Filter = "Json files (*.json)|*.json|All files (*.*)|*.*"};
            if (saveFileDialog.ShowDialog() != true)
            {
                return;
            }

            _savePath = saveFileDialog.FileName;
            SaveInternal();
        }

        public void SaveInternal()
        {

            var allItems = AllButtons.SelectMany(l => l).ToList();

            var serializedContents = new List<string>();
            foreach (var buttonViewModel in allItems)
            {
                serializedContents.Add(buttonViewModel.Serialize());
            }
            var content = "[" + Environment.NewLine + string.Join(",", serializedContents) + "]";
            File.WriteAllText(_savePath, content);
            var binaryFilename = Path.ChangeExtension(_savePath, ".bin");
            using var binWriter = new BinaryWriter(File.Open(binaryFilename, FileMode.Create));
            Serialize(binWriter);
        }

        private static void SetRowColumnForButtons(List<ButtonViewModel> viewModels)
        {
            var row = 0;
            for (var j = 0; j < 30; j +=5)
            {
                var col = 0;
                for (var i = j; i < j+5 ; i++)
                {
                    if (i >= viewModels.Count)
                    {
                        return;
                    }
                    viewModels[i].Column = col;
                    viewModels[i].Row = row;
                    col++;
                }
                row++;
            }
        }

        private static void PopulateButtons(List<ButtonViewModel> viewModels, int maxModels, string baseName)
        {
            var row = 0;
            var counter = 1;
            var factory = ServiceDirectory.Instance.GetService<IButtonViewModelFactoryService>();
            for (var j = 0; j < 30; j += 5)
            {
                var col = 0;
                for (var i = j; i < j + 5; i++)
                {
                    if (i >= maxModels)
                    {
                        return;
                    }

                    var name = $"{baseName}{counter++}";
                    if (maxModels == 1)
                    {
                        name = baseName;
                    }
                    var vm = factory.CreateViewModel(name, col, row);
                    viewModels.Add(vm);
                    col++;
                }
                row++;
            }
        }

        public IEnumerable<IEnumerable<ButtonViewModel>> AllButtons
        {
            get
            {
                yield return _slideButtons;
                yield return _analogButtons;
                yield return _sensorButtons;
                yield return _temperatureButtons;
                yield return _timerButtons;
                yield return _inputButtons;
                yield return _keyButtons;
                yield return _eventButtons;
            }
        }

        private const byte Version = 1;
        public void Serialize(BinaryWriter writer)
        {
            //first two Bytes are empty
            writer.Write((byte)0);
            writer.Write((byte)0);
            writer.Write((byte)Version);
            foreach (var keyBtn in _keyButtons)
            {
                writer.Write(keyBtn.GetEquipmentTypeCode());
            }

            writer.Seek(51, SeekOrigin.Begin);
            

            foreach (var keyButtons in AllButtons)
            {
                foreach (var keyBtn in keyButtons)
                {
                    keyBtn.Serialize(writer);
                }
            }

            //Serialize additional sequence keys
            foreach (var keyBtn in _keyButtons.Where(b=> b.EquipmentType == Constants.SEQUENTIAL && b.NumSequences > 0 && b.IsSubKey))
            {
                foreach (var buttonViewModel in keyBtn.SubButtons)
                {
                    if (buttonViewModel.EquipmentType == null)
                    {
                        buttonViewModel.EquipmentType = Constants.NOTUSE;
                    }
                    buttonViewModel.Serialize(writer);
                }
            }
        }

        public void Deserialize(BinaryReader reader)
        {
            var b = reader.ReadByte();
            b = reader.ReadByte();
            b = reader.ReadByte();

            //21 pattern types
            foreach (var keyBtn in _keyButtons)
            {
                keyBtn.SetEquipmentTypeCode(reader.ReadByte());
               
            }

            //Skip over the reserved fields
            reader.BaseStream.Seek(51, SeekOrigin.Begin);
            foreach (var keyButtons in AllButtons)
            {
                foreach (var keyBtn in keyButtons)
                {
                    keyBtn.Deserialize(reader);
                }
            }

            //deserialize additional sequence keys
            foreach (var keyBtn in _keyButtons.Where(b => b.EquipmentType == Constants.SEQUENTIAL && b.NumSequences > 0 && b.IsSubKey))
            {
                foreach (var buttonViewModel in keyBtn.SubButtons)
                {
                    buttonViewModel.Deserialize(reader);
                }
            }
        }


        public void OnEditorEvent(EditorEventTypes subsType)
        {
            if (subsType != EditorEventTypes.ContentChanged)
            {
                return;
            }
        }

        public IEnumerable<IButton> Buttons(object selectedBtn)
        {
            if (!(selectedBtn is ButtonViewModel btn))
            {
                return Enumerable.Empty<IButton>();
            }

            ButtonViewModel[] currentBtns = new[] {btn};
           
            foreach (var buttonList in AllButtons)
            {
                if (!buttonList.Any())
                {
                    continue;
                }

                if (buttonList.First().ButtonType == btn.ButtonType)
                {
                    return buttonList.Except(currentBtns).Where(b=> b.IsEnabled);
                }
            }
           
            return Enumerable.Empty<IButton>();
        }
    }
}