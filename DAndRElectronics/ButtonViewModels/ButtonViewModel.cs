using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using Common;
using Common.Services;
using Common.Enums;
using Common.Helpers;
using DAndRElectronics.Services;
using DAndRElectronics.View;
using Newtonsoft.Json;

namespace DAndRElectronics.ButtonViewModels
{
    public class ButtonViewModel:ViewModel, IOutputs
    {
        public const int MaxOuts = 22;
        public const int MaxKeys = 21;
        [JsonIgnore] private List<OutViewModel> _outViewModels = new List<OutViewModel>();
        [JsonIgnore] private bool _tempCentigrade = true;

        #region Json properties

        [JsonProperty(PropertyName = Constants.JsonButtonName)] private string _buttonName;
        [JsonProperty(PropertyName = Constants.JsonName)] private string _name;
        [JsonProperty(PropertyName = Constants.JsonEquipmentType)] private string _equipmentType = Constants.NOTUSE;
        [JsonProperty(PropertyName = Constants.JsonPriority)] private int _priority;
        [JsonProperty(PropertyName = Constants.JsonOffBackgroundColor)] private int _offBackgroundColor;
        [JsonProperty(PropertyName = Constants.JsonOnBackgroundColor)] private int _onBackgroundColor;
        [JsonProperty(PropertyName = Constants.JsonPattern)] private int _pattern;
        [JsonProperty(PropertyName = Constants.JsonDelayTime)] private Int32 _delayTime;
        [JsonProperty(PropertyName = Constants.JsonTone)] private int _tone;
        [JsonProperty(PropertyName = Constants.JsonLed1)] private int _led1;
        [JsonProperty(PropertyName = Constants.JsonLed2)] private int _led2;
        [JsonProperty(PropertyName = Constants.JsonLed3)] private int _led3;

        [JsonProperty(PropertyName = Constants.JsonSyncTone)] private bool _SyncTone;
        [JsonProperty(PropertyName = Constants.JsonSyncLed1)] private bool _SyncLed1;
        [JsonProperty(PropertyName = Constants.JsonSyncLed2)] private bool _SyncLed2;
        [JsonProperty(PropertyName = Constants.JsonSyncLed3)] private bool _SyncLed3;

        [JsonProperty(PropertyName = Constants.JsonVoltage)] private int _voltage;
        [JsonProperty(PropertyName = Constants.JsonVoltageGt)] protected bool _voltageGreaterThan;
        [JsonProperty(PropertyName = Constants.JsonTimer)] private int _timer;
        [JsonProperty(PropertyName = Constants.JsonG)] private float _gValue;
        [JsonProperty(PropertyName = Constants.JsonTemperature)] private float _temperature;
        [JsonProperty(PropertyName = Constants.JsonEvent)] private int _eventNr;

        [JsonProperty(PropertyName = Constants.JsonOuts)] private bool[] _outs = new bool[MaxOuts];
        [JsonProperty(PropertyName = Constants.JsonOutsPercent)] private int[] _outPercents = new int[MaxOuts];
        [JsonProperty(PropertyName = Constants.JsonOutsKeys)] private int[] _outsKeys = Enumerable.Repeat(2, MaxKeys).ToArray();//Not use
        [JsonProperty(PropertyName = Constants.JsonNumSequence)] private int _numSequences;
        [JsonProperty(PropertyName = Constants.JsonSequence)] public ObservableCollection<ButtonViewModel> SubButtons { get; set; } = new ObservableCollection<ButtonViewModel>();
        [JsonProperty(PropertyName = Constants.JsonSync)] private bool _sync;
        [JsonProperty(PropertyName = Constants.JsonIgnition)] private int _ignition = 2;//Not used


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
                if (!OutTabVisible) yield return Constants.JsonOutsPercent;
                if (!OutTabVisible) yield return Constants.JsonOutsKeys;
                if (!OutTabVisible) yield return Constants.JsonOuts;
                if (!PriorityVisible) yield return Constants.JsonPriority;
                if (!SyncVisible) yield return Constants.JsonSync;
                if (SubButtons.Count <= 1) yield return Constants.JsonSequence;
            }
        }

        #region Public properties

        

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
        [JsonIgnore] public bool OutTabVisible { get; set; } = true;
        [JsonIgnore] public bool PriorityVisible { get; set; } = true;
        [JsonIgnore] public bool SyncVisible { get; set; } = true;
        [JsonIgnore] public bool CanDelete { get; set; } = false;

        public bool IsSubKey { get; set; }

        [JsonIgnore]public int Priority { get => _priority; set => _priority = value; }
        [JsonIgnore]public int Tone { get => _tone; set => _tone = value; }
        [JsonIgnore]public int LED1 { get => _led1; set => _led1 = value; }
        [JsonIgnore]public int LED2 { get => _led2; set => _led2 = value; }
        [JsonIgnore]public int LED3 { get => _led3; set => _led3 = value; }

        [JsonIgnore]public bool SyncTone { get => _SyncTone; set => _SyncTone = value; }
        [JsonIgnore]public bool SyncLED1 { get => _SyncLed1; set => _SyncLed1 = value; }
        [JsonIgnore]public bool SyncLED2 { get => _SyncLed2; set => _SyncLed2 = value; }
        [JsonIgnore]public bool SyncLED3 { get => _SyncLed3; set => _SyncLed3 = value; }

        [JsonIgnore]public int Voltage { get => _voltage; set => _voltage = value; }
        [JsonIgnore]public int Timer { get => _timer; set => _timer = value; }
        [JsonIgnore]public int EventNr { get => _eventNr; set => _eventNr = value; }

        [JsonIgnore]
        public string Ignition
        {
            get => Constants.OnOffNotUseMappingsReversed[_ignition];
            set => _ignition = Constants.OnOffNotUseMappings[value];
        }

        [JsonIgnore]
        public bool Sync
        {
            get => _sync;
            set => _sync = value;
        }


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


        
        [JsonIgnore] public ICommand OffColorPickerCommand { get; }
        [JsonIgnore] public ICommand OnColorPickerCommand { get; }
        [JsonIgnore] public ICommand AllOnOffCommand { get; }
        [JsonIgnore] public ICommand VoltageSignCommand { get; }
        [JsonIgnore] public ICommand TemperatureSignCommand { get; }

        [JsonIgnore] public string ButtonName { get => _buttonName; set => _buttonName = value; }
        [JsonIgnore] public virtual string DisplayButtonName => _buttonName;
        [JsonIgnore] public string VoltageSign => _voltageGreaterThan ? " > " :  " < ";
        [JsonIgnore] public string TemperatureSign => _tempCentigrade ? " C " :  " F ";

        [JsonIgnore] public IEnumerable<OutViewModel> Outs => _outViewModels;

        [JsonIgnore] public int Column { get; set; }
        [JsonIgnore] public int Row { get; set; }

        [JsonIgnore] private Color _offColor =Colors.Transparent;
        [JsonIgnore]public Color OffColor
        {
            get
            {
                if(_offColor == Colors.Transparent)
                {
                    _offColor = IntToColor(_offBackgroundColor);
                }

                return _offColor;
            }
            set
            {
                _offColor = value;
                _offBackgroundColor = _offColor.R << 16 | _offColor.G << 8 | _offColor.B;
            }
        }

        private static Color IntToColor(int colorValue)
        {
            var blueValue = colorValue / 65536;
            var greenValue = (colorValue - blueValue * 65536) / 256;
            var redValue = colorValue - blueValue * 65536 - greenValue * 256;
            var color = Color.FromRgb(Convert.ToByte(redValue), Convert.ToByte(greenValue), Convert.ToByte(blueValue));
            return color;
        }

        [JsonIgnore] private Color _onColor = Colors.Transparent;
        [JsonIgnore]
        public Color OnColor
        {
            get
            {
                if (_onColor == Colors.Transparent)
                {
                    _onColor = IntToColor(_onBackgroundColor);
                }

                return _onColor;
            }
            set
            {
                _onColor = value;
                _onBackgroundColor = _onColor.R << 16 | _onColor.G << 8 | _onColor.B;
            }
        }
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
       

        [JsonIgnore] public static IEnumerable<int> PossibleTones { get; } = Enumerable.Range(1, 127);
        [JsonIgnore] public static IEnumerable<int> PossibleLedValues { get; } = Enumerable.Range(1, 127);

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
        [JsonIgnore] public static IEnumerable<int> PossibleTimerValues => Constants.RangedEnumeration(5, 240, 5);
        public IEnumerable<string> PossibleOnOffNotUsedValues => Constants.OnOffNotUseMappings.Keys;

        #endregion


        #region Contructors

        public ButtonViewModel(string buttonName, int col, int row)
        {
            ButtonName = buttonName;
            Column = col;
            Row = row;
            Array.Fill(_outPercents, 100);
            AllOnOffCommand = new RelayCommand(OnAllOnOrOff, AllOnOffEnabled);
            OffColorPickerCommand = new RelayCommand(OnOffColor);
            OnColorPickerCommand = new RelayCommand(OnOnColor);
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
                outViewModel.Percent = 0;
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
                vm.SubButtons.Clear();
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

        
        private void OnOffColor(object obj)
        {
            var color = PickColor();
            if (color == null)
            {
                return;
            }

            OffColor = color.Value;
            OnPropertyChanged(nameof(OffColor));
        }
        private void OnOnColor(object obj)
        {
            var color = PickColor();
            if (color == null)
            {
                return;
            }

            OnColor = color.Value;
            OnPropertyChanged(nameof(OnColor));
        }

        private Color? PickColor()
        {
            System.Windows.Forms.ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();
            colorDialog.SolidColorOnly = true;
            if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return Color.FromRgb(colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B);
            }

            return null;
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

        public virtual void Serialize(BinaryWriter writer)
        {
            SpecialHandlingAtBeginning(writer);
            //a6 Bytes
            writer.Write(GetNameInChars(),0,16);
            writer.Write((byte)Priority);
            SerializeColors(writer);
            var pattern = Sync ? Pattern + 128 : Pattern;
            writer.Write((byte)pattern);
            writer.Write((byte)DelayTime);
            writer.Write(GetSyncLedToneValue(_SyncLed1, _led1));
            writer.Write(GetSyncLedToneValue(_SyncLed2, _led2));
            writer.Write(GetSyncLedToneValue(_SyncLed3, _led3));
            writer.Write(GetSyncLedToneValue(_SyncTone, _tone));
            writer.Write((byte)_ignition);
            for (var i = 0; i < MaxOuts - 1; i++)
            {
                if (_outs[i])
                {
                    writer.Write((byte)_outPercents[i]);
                }
                else
                {
                    writer.Write((byte)0);
                }
            }
            //For the last key, write 0 or 100
            writer.Write((byte)(_outs[MaxOuts - 1] ? 100: 0));

            for (var i = 0; i < MaxKeys ; i++)
            {
                writer.Write((byte)_outsKeys[i]);
            }

            

            SpecialHandlingAtEnd(writer);
        }


        protected virtual void SpecialHandlingAtBeginning(BinaryWriter writer)
        {

        }
        protected virtual void SpecialHandlingAtEnd(BinaryWriter writer)
        {

        }

        public void Deserialize(BinaryReader reader)
        {
            var nameChars = reader.ReadChars(16);
            if (nameChars.Length == 0 || nameChars[0] == '\0')
            {
                Name = null;
            }
            else
            {
                Name = new string(nameChars);
            }
           
            Priority = reader.ReadByte();
            DeserializeColors(reader);
            var pattern =  reader.ReadByte();
            if (pattern >= 128)
            {
                Pattern = pattern - 128;
                Sync = true;
            }
            else
            {
                Pattern = pattern;
                Sync = false;
            }
            DelayTime = reader.ReadByte();
            
            SetSyncLedToneValue(reader, ref _SyncLed1, ref _led1);
            SetSyncLedToneValue(reader, ref _SyncLed2, ref _led2);
            SetSyncLedToneValue(reader, ref _SyncLed3, ref _led3);
            SetSyncLedToneValue(reader, ref _SyncTone, ref _tone);
            _ignition = reader.ReadByte();

            for (var i = 0; i < MaxOuts - 1; i++)
            {
                var o = reader.ReadByte();
                if (o > 0)
                {
                    _outs[i] = true;
                    _outPercents[i] = o;
                }
                else
                {
                    _outs[i] = false;
                    _outPercents[i] = 0;
                }
            }
            //For the last key, it will always be 0 or 100
            var b = reader.ReadByte();
            if (b > 0)
            {
                _outPercents[MaxOuts - 1] = 100;
                _outs[MaxOuts - 1] = true;
            }
            else
            {
                _outPercents[MaxOuts - 1] = 0;
                _outs[MaxOuts - 1] = false;
            }
            

            for (var i = 0; i < MaxKeys; i++)
            {
                _outsKeys[i] = reader.ReadByte();
            }
        }

        protected virtual void SerializeColors(BinaryWriter writer)
        {
            var colorConverter = new ColorHelper(OffColor);
            colorConverter.Serialize(writer);
            colorConverter.Color = OnColor;
            colorConverter.Serialize(writer);
        }

        protected virtual void DeserializeColors(BinaryReader reader)
        {
            var colorConverter = new ColorHelper(reader);
            OffColor = colorConverter.Color;
            colorConverter = new ColorHelper(reader);
            OnColor = colorConverter.Color;
        }

        protected void WriteFiveBytes(BinaryWriter writer)
        {
            for (var i = 0; i < 5; i++)
            {
                writer.Write((byte)0);
            }
        }
        protected void ReadFiveBytes(BinaryReader reader)
        {
            for (var i = 0; i < 5; i++)
            {
                reader.ReadByte();
            }
        }

        private byte GetSyncLedToneValue(bool sync, int value)
        {
            if (sync)
            {
                value += 128;
            }

            return (byte) value;
        }

        private void SetSyncLedToneValue(BinaryReader reader,  ref bool sync, ref int value)
        {
            var b = reader.ReadByte();
            if (b >= 128)
            {
                sync = true;
                value = b - 128;
            }
            else
            {
                value = b;
            }
        }

        private char[] GetNameInChars()
        {
            var name = new char[16];
            if (string.IsNullOrEmpty(Name))
            {
                return name;
            }
            for(var i=0; i <16; i++)
            {
                if (i >= Name.Length)
                {
                    break;
                }

                name[i] = Name[i];
            }

            return name;
        }

        public byte GetEquipmentTypeCode()
        {
            switch (_equipmentType)
            {
                case Constants.MOMENTARY:
                    return 16;
                case Constants.TOGGLE:
                    return 32;
                case Constants.DELAY:
                    return 64;
                case Constants.NOTUSE:
                    return 0;
                    
                case Constants.SEQUENTIAL:
                    return (byte)(128 + NumSequences);
                    break;
                default:
                    return 0;
            }
        }

        public void SetEquipmentTypeCode(byte code)
        {
            switch (code)
            {
                case 16:
                    _equipmentType = Constants.MOMENTARY;
                    break;
                case 32:
                    _equipmentType = Constants.TOGGLE;
                    break;
                case 64:
                    _equipmentType = Constants.DELAY;
                    break;
                case 0:
                    _equipmentType = Constants.NOTUSE;
                    break;
                default:
                    if (code >= 128)
                    {
                        _equipmentType = Constants.SEQUENTIAL;
                        NumSequences = code - 128;
                    }
                    break;
            }
        }

    }
}