using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using DAndRElectronics.Helpers;
using DAndRElectronics.Services;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace DAndRElectronics.View
{
    public class KeyboardViewModel:ViewModel
    {
        private const string KeyBaseName = "KEY";
        private const string InputBaseName = "INPUT";
        private const string EventBaseName = "CAN EVENT";

        private List<ButtonViewModel> _keyButtons = new List<ButtonViewModel>();
        private List<ButtonViewModel> _inputButtons = new List<ButtonViewModel>();
        private List<ButtonViewModel> _eventButtons = new List<ButtonViewModel>();
        public IEnumerable<ButtonViewModel> KeyButtons => _keyButtons;
        public IEnumerable<ButtonViewModel> InputButtons => _inputButtons;
        public IEnumerable<ButtonViewModel> EventButtons => _eventButtons;
        public ICommand SaveCommand { get; }
        public ICommand SaveAsCommand { get; }
        public ICommand OpenCommand { get; }
        

        #region Contructors

        public KeyboardViewModel()
        {
            SaveCommand = new RelayCommand(OnSave);
            SaveAsCommand = new RelayCommand(OnSaveAs);
            OpenCommand = new RelayCommand(OnOpen);
            PopulateButtons(_keyButtons, 21, KeyBaseName);
            PopulateButtons(_inputButtons, 16, InputBaseName);
            PopulateButtons(_eventButtons, 5, EventBaseName);
            //PopulateKeyButtons();
            //PopulateInputButtons();
            //PopulateEventButtons();
        }

        private void OnSave(object obj)
        {
            
        }
        private void OnOpen(object obj)
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Json files (*.json)|*.json|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() != true)
            {
                return;
            }

            var content = File.ReadAllText(openFileDialog.FileName);
            var allItems = JsonConvert.DeserializeObject<List<ButtonViewModel>>(content);
            _keyButtons = allItems.Where(i => i.ButtonName.StartsWith(KeyBaseName)).ToList();
            _inputButtons = allItems.Where(i => i.ButtonName.StartsWith(InputBaseName)).ToList();
            _eventButtons = allItems.Where(i => i.ButtonName.StartsWith(EventBaseName)).ToList();
            var stateService = ServiceDirectory.Instance.GetService<IStateService>();
            SetRowColumnForButtons(_keyButtons);
            SetRowColumnForButtons(_inputButtons);
            SetRowColumnForButtons(_eventButtons);
            stateService.OnStateChanged(StateChangedTypes.ProjectOpened);

        }
        private void OnSaveAs(object obj)
        {

            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Json files (*.json)|*.json|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() != true)
            {
                return;
            }

            var allItems = _keyButtons.Concat(_inputButtons).Concat(_eventButtons).ToList();
            File.WriteAllText(saveFileDialog.FileName, JsonConvert.SerializeObject(allItems, Formatting.Indented));

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
            for (var j = 0; j < 30; j += 5)
            {
                var col = 0;
                for (var i = j; i < j + 5; i++)
                {
                    if (i >= maxModels)
                    {
                        return;
                    }

                    var vm = new ButtonViewModel($"{baseName}{counter++}", col, row);
                    viewModels.Add(vm);
                    col++;
                }
                row++;
            }
        }


        #endregion
    }
}