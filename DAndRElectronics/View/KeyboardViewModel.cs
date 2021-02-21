using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using DAndRElectronics.Helpers;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace DAndRElectronics.View
{
    public class KeyboardViewModel:ViewModel
    {
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
            PopulateKeyButtons();
            PopulateInputButtons();
            PopulateEventButtons();
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
            _keyButtons = allItems.Where(i => i.ButtonName.StartsWith("Key")).ToList();
            _inputButtons = allItems.Where(i => i.ButtonName.StartsWith("Input")).ToList();
            _eventButtons = allItems.Where(i => i.ButtonName.StartsWith("Can")).ToList();
            OnPropertyChanged(nameof(KeyButtons));
            OnPropertyChanged(nameof(InputButtons));
            OnPropertyChanged(nameof(EventButtons));

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

        private void PopulateKeyButtons()
        {

            var row = 0;
            var col = 0;
            _keyButtons.Add(new ButtonViewModel("KEY1", col, row));
            _keyButtons.Add(new ButtonViewModel("KEY2", ++col, row));
            _keyButtons.Add(new ButtonViewModel("KEY3", ++col, row));
            _keyButtons.Add(new ButtonViewModel("KEY4", ++col, row));
            _keyButtons.Add(new ButtonViewModel("KEY5", ++col, row));
            col = 0;
            row++;
            _keyButtons.Add(new ButtonViewModel("KEY6",  col, row));
            _keyButtons.Add(new ButtonViewModel("KEY7",  ++col, row));
            _keyButtons.Add(new ButtonViewModel("KEY8",  ++col, row));
            _keyButtons.Add(new ButtonViewModel("KEY9",  ++col, row));
            _keyButtons.Add(new ButtonViewModel("KEY10", ++col, row));
            col = 0;
            row++;
            _keyButtons.Add(new ButtonViewModel("KEY6",  col, row));
            _keyButtons.Add(new ButtonViewModel("KEY7",  ++col, row));
            _keyButtons.Add(new ButtonViewModel("KEY8",  ++col, row));
            _keyButtons.Add(new ButtonViewModel("KEY9",  ++col, row));
            _keyButtons.Add(new ButtonViewModel("KEY10", ++col, row));
            col = 0;
            row++;
            _keyButtons.Add(new ButtonViewModel("KEY11", col, row));
            _keyButtons.Add(new ButtonViewModel("KEY12", ++col, row));
            _keyButtons.Add(new ButtonViewModel("KEY13", ++col, row));
            _keyButtons.Add(new ButtonViewModel("KEY14", ++col, row));
            _keyButtons.Add(new ButtonViewModel("KEY15", ++col, row));
            col = 0;
            row++;
            _keyButtons.Add(new ButtonViewModel("KEY16", col, row));
            _keyButtons.Add(new ButtonViewModel("KEY17", ++col, row));
            _keyButtons.Add(new ButtonViewModel("KEY18", ++col, row));
            _keyButtons.Add(new ButtonViewModel("KEY19", ++col, row));
            _keyButtons.Add(new ButtonViewModel("KEY20", ++col, row));



        }


        private void PopulateInputButtons()
        {
            var row = 0;
            var col = 0;
            _inputButtons.Add(new ButtonViewModel("Input1", col, row));
            _inputButtons.Add(new ButtonViewModel("Input2", ++col, row));
            _inputButtons.Add(new ButtonViewModel("Input3", ++col, row));
            _inputButtons.Add(new ButtonViewModel("Input4", ++col, row));
            _inputButtons.Add(new ButtonViewModel("Input5", ++col, row));
            col = 0;
            row++;
            _inputButtons.Add(new ButtonViewModel("Input6", col, row));
            _inputButtons.Add(new ButtonViewModel("Input7", ++col, row));
            _inputButtons.Add(new ButtonViewModel("Input8", ++col, row));
            _inputButtons.Add(new ButtonViewModel("Input9", ++col, row));
            _inputButtons.Add(new ButtonViewModel("Input10", ++col, row));
            col = 0;
            row++;
            _inputButtons.Add(new ButtonViewModel("Input6", col, row));
            _inputButtons.Add(new ButtonViewModel("Input7", ++col, row));
            _inputButtons.Add(new ButtonViewModel("Input8", ++col, row));
            _inputButtons.Add(new ButtonViewModel("Input9", ++col, row));
            _inputButtons.Add(new ButtonViewModel("Input10", ++col, row));
            col = 0;
            row++;
            _inputButtons.Add(new ButtonViewModel("Input11", col, row));
            _inputButtons.Add(new ButtonViewModel("Input12", ++col, row));
            _inputButtons.Add(new ButtonViewModel("Input13", ++col, row));
            _inputButtons.Add(new ButtonViewModel("Input14", ++col, row));
            _inputButtons.Add(new ButtonViewModel("Input15", ++col, row));
            col = 0;
            row++;
            _inputButtons.Add(new ButtonViewModel("Input16", col, row));
        }


        private void PopulateEventButtons()
        {
            var row = 0;
            var col = 0;
            _eventButtons.Add(new ButtonViewModel("Can Event1", col, row));
            _eventButtons.Add(new ButtonViewModel("Can Event2", ++col, row));
            _eventButtons.Add(new ButtonViewModel("Can Event3", ++col, row));
            _eventButtons.Add(new ButtonViewModel("Can Event4", ++col, row));
            _eventButtons.Add(new ButtonViewModel("Can Event5", ++col, row));
           
        }

        #endregion
    }
}