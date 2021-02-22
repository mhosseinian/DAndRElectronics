using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using DAndRElectronics.Helpers;
using Newtonsoft.Json;

namespace DAndRElectronics.View
{
    public enum SupportedColors
    {
        Black = 0,
        Red=1,
        Green=2,
        Blue=3,
        Yellow=4,
        White=5
    }

    

    public class ButtonViewModel:ViewModel
    {
        private const int MaxOuts = 21;
        [JsonProperty(PropertyName = "buttonName")] private string _buttonName;
        [JsonProperty(PropertyName = "name")] private string _name;
        [JsonProperty(PropertyName = "equipmentType")] private string _equipmentType;
        [JsonProperty(PropertyName = "priority")] private int _priority;
        [JsonProperty(PropertyName = "offBackgroundColor")] private int _offBackgroundColor;
        [JsonProperty(PropertyName = "onBackgroundColor")] private int _onBackgroundColor;
        [JsonProperty(PropertyName = "pattern")] private string _pattern;
        [JsonProperty(PropertyName = "outs")] private bool[] _outs = new bool[MaxOuts];
        [JsonProperty(PropertyName = "delayTime")] private Int32 _delayTime;


        [JsonIgnore] private List<OutViewModel> _outViewModels = new List<OutViewModel>();
        [JsonIgnore]public string Name { get => _name; set => _name = value; }

        [JsonIgnore]
        public string EquipmentType
        {
            get => _equipmentType;
            set
            {
                _equipmentType = value;
                PopulateAdditionalViewModels();
                OnPropertyChanged(nameof(SubButtonsEnabled));
                OnPropertyChanged(nameof(DelayVisible));
            }
        }

        private void PopulateAdditionalViewModels()
        {
            if (SubButtons.Count == 1 && SubButtonsEnabled)
            {
                var counter = 1;
                var items = new List<ButtonViewModel>
                {
                    this,
                    new ButtonViewModel($"{ButtonName}_{counter++}", 0, 0){IsSubKey = true},
                    new ButtonViewModel($"{ButtonName}_{counter++}", 0, 0){IsSubKey = true},
                    new ButtonViewModel($"{ButtonName}_{counter}", 0, 0){IsSubKey = true},
                };
                SubButtons = new ObservableCollection<ButtonViewModel>(items);
            }
            OnPropertyChanged(nameof(SubButtons));
            OnPropertyChanged(nameof(SelectedViewModel));
        }

        [JsonIgnore] private ButtonViewModel _selectedViewModel;
        [JsonIgnore]
        public ButtonViewModel SelectedViewModel
        {
            get => _selectedViewModel;
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore] public bool SubButtonsEnabled => IsSubKey || EquipmentType == EquipmentTypes.SEQUENTIAL;
        [JsonIgnore] public bool DelayVisible => EquipmentType == EquipmentTypes.DELAY;
        [JsonIgnore] public bool EquipmentTypeVisible => !IsSubKey;
        public bool IsSubKey { get; set; }
        [JsonIgnore]public int Priority { get => _priority; set => _priority = value; }

        [JsonIgnore]
        public SupportedColors OffBackgroundColor
        {
            get
            {
                return ((SupportedColors) _offBackgroundColor);
            } set => _offBackgroundColor = (int)value;
        }


        [JsonIgnore]
        public SupportedColors OnBackgroundColor
        {
            get
            {
                return ((SupportedColors)_onBackgroundColor);
            }
            set => _onBackgroundColor = (int)value;
        }

        [JsonIgnore]public string Pattern { get => _pattern; set => _pattern = value; }

        [JsonIgnore]public int DelayTime { get => _delayTime; set => _delayTime = value; }


        [JsonIgnore] public ICommand OnEdit { get; }
        [JsonIgnore] public ICommand AllOnOffCommand { get; }

        [JsonIgnore] public string ButtonName { get => _buttonName; set => _buttonName = value; }

        [JsonIgnore] public IEnumerable<OutViewModel> Outs => _outViewModels;

        [JsonIgnore] public int Column { get; set; }
        [JsonIgnore] public int Row { get; set; }
        [JsonIgnore] public static List<string> PossibleNames { get; }=new List<string>
        {
            "Ali",
            "Hassan"
        };

        [JsonIgnore] public IEnumerable<string> PossibleTypes => EquipmentTypes.PossibleTypes;

        [JsonIgnore]
        public IEnumerable<int> PossibleDelays
        {
            get
            {
                return ButtonName.StartsWith(KeyboardViewModel.AnalogBaseName) ? Enumerable.Range(1, 34) : Enumerable.Range(1, 200);
            }
        }

        [JsonIgnore] public static List<int> PossiblePriorities { get; } = new List<int> {1, 2, 3, 4, 5, 6, 7, 8, 9};
        
        [JsonIgnore] public static List<string> PossibleColors { get; }=new List<string>
        {
            "Black",
            "Red",
            "Green",
            "Blue",
            "Yellow",
        };

        [JsonIgnore] public static List<string> PossiblePatterns { get; }=new List<string>
        {
            "Pattern 1",
            "p 2",
            
        };

        [JsonIgnore]public ObservableCollection<ButtonViewModel> SubButtons { get; set; } = new ObservableCollection<ButtonViewModel>();

        #region Contructors

        public ButtonViewModel(string buttonName, int col, int row)
        {
            ButtonName = buttonName;
            Column = col;
            Row = row;
            OnEdit = new RelayCommand(OnButtonEdit, o => true);
            AllOnOffCommand = new RelayCommand(OnAllOnOrOff, AllOnOffEnabled);
            PopulateOutViewModels();
            SubButtons.Add(this);
            SelectedViewModel = this;
        }

        private bool AllOnOffEnabled(object obj)
        {
            return true;
        }

        private void OnAllOnOrOff(object obj)
        {
            var onOrOff = (bool)obj;
            foreach (var vm in _outViewModels)
            {
                vm.On = onOrOff;
            }
        }

        #endregion

        private void OnButtonEdit(object obj)
        {
            
        }

        public static ButtonViewModel Deserialize(string content)
        {
            var vmNew = JsonConvert.DeserializeObject<ButtonViewModel>(content);
            return vmNew;
        }

        public void PopulateOutViewModels()
        {
            _outViewModels.Clear();

            for (var i = 0; i < MaxOuts; i++)
            {
                var vm = new OutViewModel(i, index => _outs[index], (index, value) => _outs[index] = value );
                _outViewModels.Add(vm);
            }
        }

    }
}