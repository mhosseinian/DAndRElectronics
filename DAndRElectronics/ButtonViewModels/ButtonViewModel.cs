using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using DAndRElectronics.Enums;
using DAndRElectronics.Helpers;
using DAndRElectronics.Services;
using DAndRElectronics.View;
using Newtonsoft.Json;

namespace DAndRElectronics.ButtonViewModels
{
    public class ButtonViewModel:ViewModel, IOutputs
    {
        private const int MaxOuts = 21;
        [JsonIgnore] private List<OutViewModel> _outViewModels = new List<OutViewModel>();
        [JsonIgnore] private bool _tempCentigrade = true;

        #region Json properties

        [JsonProperty(PropertyName = Constants.JsonButtonName)] private string _buttonName;
        [JsonProperty(PropertyName = Constants.JsonName)] private string _name;
        [JsonProperty(PropertyName = Constants.JsonEquipmentType)] private string _equipmentType;
        [JsonProperty(PropertyName = Constants.JsonPriority)] private int _priority;
        [JsonProperty(PropertyName = Constants.JsonOffBackgroundColor)] private int _offBackgroundColor;
        [JsonProperty(PropertyName = Constants.JsonOnBackgroundColor)] private int _onBackgroundColor;
        [JsonProperty(PropertyName = Constants.JsonPattern)] private int _pattern;
        [JsonProperty(PropertyName = Constants.JsonDelayTime)] private Int32 _delayTime;
        [JsonProperty(PropertyName = Constants.JsonTone)] private int _tone;
        [JsonProperty(PropertyName = Constants.JsonLed1)] private int _led1;
        [JsonProperty(PropertyName = Constants.JsonLed2)] private int _led2;
        [JsonProperty(PropertyName = Constants.JsonLed3)] private int _led3;
        [JsonProperty(PropertyName = Constants.JsonVoltage)] private int _voltage;
        [JsonProperty(PropertyName = Constants.JsonVoltageGt)] private bool _voltageGreaterThan;
        [JsonProperty(PropertyName = Constants.JsonTimer)] private int _timer;
        [JsonProperty(PropertyName = Constants.JsonG)] private float _gValue;
        [JsonProperty(PropertyName = Constants.JsonTemperature)] private float _temperature;
        [JsonProperty(PropertyName = Constants.JsonEvent)] private int _eventNr;

        [JsonProperty(PropertyName = Constants.JsonOuts)] private bool[] _outs = new bool[MaxOuts];
        [JsonProperty(PropertyName = Constants.JsonOutsPercent)] private int[] _outPercents = new int[MaxOuts];
        [JsonProperty(PropertyName = Constants.JsonOutsKeys)] private int[] _outsKeys = new int[MaxOuts];

        #endregion


        #region public methods

        public virtual ButtonViewModel Deserialize(string content)
        {
            return JsonConvert.DeserializeObject<ButtonViewModel>(content);
        }

        protected void CheckButtonName(string buttonName, string expectedName)
        {
            if (!buttonName.StartsWith(expectedName))
            {
                throw new ArgumentException($"button name must start with:{expectedName}");
            }
        }

        #endregion

        [JsonIgnore]
        public IEnumerable<string> IgnoreProperties
        {
            get
            {
                if (!EquipmentTypeVisible) yield return Constants.JsonEquipmentType;
                if (!NameVisible) yield return Constants.JsonName;
                if (!PatternVisible) yield return Constants.JsonPattern;
                if (!OffColorVisible) yield return Constants.JsonOffBackgroundColor;
                if (!OnColorVisible) yield return Constants.JsonOnBackgroundColor;
                if (!VoltageVisible) yield return Constants.JsonVoltage;
                if (!TimerVisible) yield return Constants.JsonTimer;
                if (!TemperatureVisible) yield return Constants.JsonTemperature;
                if (!EventVisible) yield return Constants.JsonEvent;
                if (!SensorVisible) yield return Constants.JsonG;
                if (!PercentsVisible) yield return Constants.JsonOutsPercent;
                if (!PercentsVisible) yield return Constants.JsonOutsKeys;
                if (!PriorityVisible) yield return Constants.JsonPriority;
            }
        }

        #region Public properties

        [JsonIgnore] private int _numSequences;

        [JsonIgnore]
        public int NumSequences
        {
            get => _numSequences;
            set
            {
                _numSequences = value;
                PopulateAdditionalViewModels();
            }
        }

        [JsonIgnore]public string Name { get => _name; set => _name = value; }

        [JsonIgnore]
        public string EquipmentType
        {
            get => _equipmentType;
            set
            {
                _equipmentType = value;
                if (_equipmentType != Constants.SEQUENTIAL)
                {
                    NumSequences = 0;
                }
                OnPropertyChanged(nameof(SubButtonsEnabled));
                
                OnPropertyChanged(nameof(DelayVisible));
            }
        }


        [JsonIgnore] private ButtonViewModel _selectedViewModel;
        
        [JsonIgnore]public ButtonViewModel SelectedViewModel
        {
            get => _selectedViewModel;
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore] public bool SubButtonsEnabled => IsSubKey || EquipmentType == Constants.SEQUENTIAL;
        [JsonIgnore] public bool DelayVisible => EquipmentType == Constants.DELAY;
        [JsonIgnore] public bool OutsEnabled => Pattern == 0;
        [JsonIgnore] public virtual bool EquipmentTypeVisible => !IsSubKey;
        [JsonIgnore] public bool NameVisible { get; set; } = true;
        [JsonIgnore] public bool PatternVisible { get; set; } = true;
        
        [JsonIgnore] public bool OffColorVisible { get; set; } = true;
        [JsonIgnore] public bool OnColorVisible { get; set; } = true;
        [JsonIgnore] public bool VoltageVisible { get; set; } = false;
        [JsonIgnore] public bool TimerVisible { get; set; } = false;
        [JsonIgnore] public bool TemperatureVisible { get; set; } = false;
        [JsonIgnore] public bool EventVisible { get; set; } = false;
        [JsonIgnore] public bool SensorVisible { get; set; } = false;
        [JsonIgnore] public bool PercentsVisible { get; set; } = true;
        [JsonIgnore] public bool PriorityVisible { get; set; } = true;
        [JsonIgnore] public bool CanDelete { get; set; } = false;

        public bool IsSubKey { get; set; }

        [JsonIgnore]public int Priority { get => _priority; set => _priority = value; }
        [JsonIgnore]public int Tone { get => _tone; set => _tone = value; }
        [JsonIgnore]public int LED1 { get => _led1; set => _led1 = value; }
        [JsonIgnore]public int LED2 { get => _led2; set => _led2 = value; }
        [JsonIgnore]public int LED3 { get => _led3; set => _led3 = value; }
        [JsonIgnore]public int Voltage { get => _voltage; set => _voltage = value; }
        [JsonIgnore]public int Timer { get => _timer; set => _timer = value; }
        [JsonIgnore]public int EventNr { get => _eventNr; set => _eventNr = value; }

        [JsonIgnore]
        public float GValue
        {
            get => _gValue;
            set
            {
                _gValue = value;
                OnPropertyChanged(nameof(GValueHasError));
            }
        }


        [JsonIgnore] public bool[] Outputs => _outs;
        [JsonIgnore] public int[] OutputPercents => _outPercents;
        [JsonIgnore] public int[] OutputKeys => _outsKeys;

        [JsonIgnore]
        public float Temperature
        {
            get => _tempCentigrade ? _temperature : (_temperature * 9F / 5) + 32;
            set => _temperature = value;
        }

        [JsonIgnore] public string DeleteText => $"Delete {ButtonName}";
        [JsonIgnore] public bool GValueHasError => GValue < 0 || GValue > 10;

        
        [JsonIgnore]public SupportedColors OffBackgroundColor
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

        [JsonIgnore]
        public int Pattern
        {
            get { return _pattern; }
            set
            {
                _pattern = value;
                DisableOuts();
                OnPropertyChanged(nameof(OutsEnabled));
            }
        }

        [JsonIgnore]public int DelayTime { get => _delayTime; set => _delayTime = value; }


        
        [JsonIgnore] public ICommand AllOnOffCommand { get; }
        [JsonIgnore] public ICommand VoltageSignCommand { get; }
        [JsonIgnore] public ICommand TemperatureSignCommand { get; }

        [JsonIgnore] public string ButtonName { get => _buttonName; set => _buttonName = value; }
        [JsonIgnore] public string VoltageSign => _voltageGreaterThan ? " > " :  " < ";
        [JsonIgnore] public string TemperatureSign => _tempCentigrade ? " C " :  " F ";

        [JsonIgnore] public IEnumerable<OutViewModel> Outs => _outViewModels;

        [JsonIgnore] public int Column { get; set; }
        [JsonIgnore] public int Row { get; set; }
        [JsonIgnore]public ObservableCollection<ButtonViewModel> SubButtons { get; set; } = new ObservableCollection<ButtonViewModel>();

        #endregion

        #region Possible dropdown values


        [JsonIgnore] public static List<string> PossibleNames { get; }=new List<string>
        {
            "Ali",
            "Hassan"
        };

        [JsonIgnore] public  virtual IEnumerable<string> PossibleTypes => Constants.PossibleTypes;

        [JsonIgnore]
        public IEnumerable<int> PossibleDelays
        {
            get
            {
                return ButtonName.StartsWith(Constants.AnalogBaseName) ? Enumerable.Range(1, 34) : Enumerable.Range(1, 255);
            }
        }

        [JsonIgnore] public static List<int> PossiblePriorities { get; } = new List<int> {1, 2, 3, 4, 5, 6, 7, 8, 9};
       

        [JsonIgnore] public static IEnumerable<int> PossibleTones { get; } = Enumerable.Range(1, 255);
        [JsonIgnore] public static IEnumerable<int> PossibleLedValues { get; } = Enumerable.Range(1, 255);

        [JsonIgnore] public static List<string> PossibleColors { get; }=new List<string>
        {
            "Black",
            "Red",
            "Green",
            "Blue",
            "Yellow",
        };

        [JsonIgnore] public static IEnumerable<int> PossiblePatterns  => Enumerable.Range(0, 200);
        [JsonIgnore] public static IEnumerable<int> PossibleVoltages => Enumerable.Range(0, 32);
        [JsonIgnore] public static IEnumerable<int> PossibleEvents => Enumerable.Range(1, 250);
        [JsonIgnore] public static IEnumerable<int> PossibleSequences => Enumerable.Range(0, 10);
        [JsonIgnore] public static IEnumerable<int> PossibleTimerValues => Constants.RangedEnumeration(1, 240, 5);

        #endregion


        #region Contructors

        public ButtonViewModel(string buttonName, int col, int row)
        {
            ButtonName = buttonName;
            Column = col;
            Row = row;
            
            AllOnOffCommand = new RelayCommand(OnAllOnOrOff, AllOnOffEnabled);
            VoltageSignCommand = new RelayCommand(OnVoltageSign);
            TemperatureSignCommand = new RelayCommand(OnTemperatureSign);
            PopulateOutViewModels();
            SubButtons.Add(this);
            SelectedViewModel = this;
        }

        #endregion

        #region Private methods



        private void DisableOuts()
        {
            if (OutsEnabled)
            {
                return;
            }

            foreach (var outViewModel in Outs)
            {
                outViewModel.On = false;
            }
        }

        private void PopulateAdditionalViewModels()
        {
            
            if (SubButtons.Count == 0)
            {
                SubButtons.Add(this);
            }

            if (NumSequences == 0)
            {
                SubButtons.Clear();
                SubButtons.Add(this);
                SelectedViewModel = this;
            OnPropertyChanged(nameof(SubButtons));
            OnPropertyChanged(nameof(SelectedViewModel));
                return;
            }
            if (SubButtons.Count - 1 == NumSequences)
            {
                return;
            }
            if (SubButtons.Count -1 > NumSequences)
            {
                var buttons = SubButtons.Take(NumSequences+1);
                SubButtons = new ObservableCollection<ButtonViewModel>(buttons);
                SelectedViewModel = this;
                OnPropertyChanged(nameof(SubButtons));
                OnPropertyChanged(nameof(SelectedViewModel));
                return;
            }

            var numViewsRequired = NumSequences - SubButtons.Count + 1;
            var factory = ServiceDirectory.Instance.GetService<IButtonViewModelFactoryService>();
            var counter = SubButtons.Count;
            var items = new List<ButtonViewModel>();
            
            for (var i = 0; i < numViewsRequired; i++)
            {
                var vm = factory.CreateViewModel($"{ButtonName}_{counter++}", 0, 0);
                vm.IsSubKey = true;
                SubButtons.Add(vm);
            }

            
            SelectedViewModel = this;
            OnPropertyChanged(nameof(SubButtons));
            OnPropertyChanged(nameof(SelectedViewModel));
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

        private void OnVoltageSign(object obj)
        {
            _voltageGreaterThan = !_voltageGreaterThan;
            OnPropertyChanged(nameof(VoltageSign));
        }

        private void OnTemperatureSign(object obj)
        {
            _tempCentigrade = !_tempCentigrade;
            OnPropertyChanged(nameof(TemperatureSign));
            OnPropertyChanged(nameof(Temperature));
        }


        private void PopulateOutViewModels()
        {
            _outViewModels.Clear();

            for (var i = 0; i < MaxOuts; i++)
            {
                var vm = new OutViewModel(i, this);
                _outViewModels.Add(vm);
            }
        }

        #endregion

    }
}