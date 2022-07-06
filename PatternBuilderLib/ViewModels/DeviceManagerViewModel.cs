using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Common;
using Common.Helpers;
using Newtonsoft.Json;

namespace PatternBuilderLib.ViewModels
{
    public class DeviceManagerViewModel: ViewModel
    {
        private const double StartPosition = 100.0;
        private int _width;

        [JsonProperty(PropertyName = "IsLine")]private bool _isLine;
        [JsonProperty(PropertyName = Constants.JsonDevices)]
        public List<DeviceViewModel> Devices { get; set; } = new List<DeviceViewModel>();

        

        public int NumDevices { get; set; } = 14;
        public int CycleNumber { get; set; }

        [JsonIgnore]public bool IsLine
        {
            get => _isLine;
            set
            {
                _isLine = value;
                if (!Devices.Any())
                {
                    return;
                }
                if (value)
                {
                    foreach (var vm in Devices)
                    {
                        vm.RotateAngle = 0.0;
                    }
                }
                else
                {
                    PositionDevices();
                }
                OnPropertyChanged();
            }
        }

        public string DeleteText => $"Delete Cycle {CycleNumber}";
        [JsonIgnore] public string CloneText => $"Clone Cycle {CycleNumber}";
        [JsonIgnore] public string Label => $"Cycle # {CycleNumber}";

        [JsonIgnore] public bool AllTop { get; set; }
        [JsonIgnore] public bool AllBottom { get; set; }
        [JsonIgnore] public bool AllDriver { get; set; }
        [JsonIgnore] public bool AllPassenger { get; set; }


        #region Contructors

        public DeviceManagerViewModel()
        {
        }

       

        public DeviceManagerViewModel(int numCycles, bool isLine):base()
        {
            IsLine = isLine;
            NumDevices = numCycles;
           
            for (var i = 0; i < NumDevices; i++)
            {
                Devices.Add(new DeviceViewModel{Index = i+1});
            }

            if (!IsLine)
            {
                PositionDevices();
            }
        }
        public DeviceManagerViewModel(DeviceManagerViewModel src)
        {
            CloneSource(src);
        }

        private void CloneSource(DeviceManagerViewModel src)
        {
            foreach (var deviceViewModel in src.Devices)
            {
                var vm = DeviceViewModel.Clone(deviceViewModel);
                Devices.Add(vm);
            }

            NumDevices = src.NumDevices;
            IsLine = src.IsLine;
        }

        #endregion

        private int NumHorizontals => (NumDevices - 6) / 2;

        private IEnumerable<DeviceViewModel> DriverDevices
        {
            get
            {
                var startLeftSide = (NumHorizontals * 2) + 3;
                for (var i = startLeftSide; i < startLeftSide + 3; i++)
                {
                    yield return Devices[i];
                }
            }
        }
        private IEnumerable<DeviceViewModel> PassengerDevices
        {
            get
            {
                var start = NumHorizontals;
                for (var i = start; i < start + 3; i++)
                {
                    yield return Devices[i];
                }
            }
        }
        private IEnumerable<DeviceViewModel> TopDevices
        {
            get
            {
                var end = NumHorizontals;
                for (var i = 0; i < NumHorizontals; i++)
                {
                    yield return Devices[i];
                }
            }
        }
        private IEnumerable<DeviceViewModel> BottomDevices
        {
            get
            {
                var start = NumHorizontals + 3;
                var end = start + NumHorizontals;
                for (var i = start; i < end; i++)
                {
                    yield return Devices[i];
                }
            }
        }


        public void PositionDevices()
        {
            //Always three on each side
            PositionTopSide();
            PositionPassengerSide();
            PositionBottomSide();
            PositionDriverSide();
        }

        private void PositionDriverSide()
        {
            var a = DriverDevices.First();
            var source = BottomDevices.Last();
            a.Top = source.Top;
            a.RotateAngle = 45;
            a.Left = source.Left - source.Width;
            a.Origin = new Point(1,0);

            var b = DriverDevices.Skip(1).First();
            var m = a.Width * 0.6494480;//Sin(45)
            b.Top = a.Top - m - a.Width;
            b.RotateAngle = 90;
            b.Left = a.Left + (a.Width * 0.76) - a.Height;//Cos(45)
            b.Origin = new Point(0, 0);


            source = TopDevices.First();
            var c = DriverDevices.Last();
            c.Top = source.Top;
            c.RotateAngle = -45;
            c.Left = source.Left - source.Width;
            c.Origin = new Point(1,1);
        }

        private void PositionTopSide()
        {
            var left = StartPosition;
            for (var i = 0; i < NumHorizontals; i++)
            {
                Devices[i].Left = left;
                left += Devices[i].Width;
            }
        }

        private void PositionBottomSide()
        {
            var numHorizontal = NumHorizontals;
            double left;
            var c = PassengerDevices.Last();
            var top = c.Width * 0.6494480 + c.Top - c.Height / 2;
            foreach (var device in BottomDevices)
            {
                device.Top = top;
            }

            var start = (numHorizontal * 2) + 2;
            var endLoop = start - numHorizontal + 1;
            left = StartPosition;
            for (var i = start; i >= endLoop; i--)
            {
                Devices[i].Left = left;
                left += Devices[i].Width;
            }
        }

        private void PositionPassengerSide()
        {
            var a = PassengerDevices.First();
            var source = TopDevices.Last();
            a.Top = source.Top;
            a.Left = source.Left + source.Width;
            a.Origin = new Point(0, 1);
            a.RotateAngle = 45;
            var b = PassengerDevices.Skip(1).First();
            var m = a.Width * 0.6494480;
            b.Top = m + a.Top;
            b.RotateAngle = 90;
            b.Left = a.Left + (a.Width * 0.76) - a.Height / 4;
            b.Origin = new Point(0, 1);

            var c = PassengerDevices.Last();
            c.Top = b.Top + b.Width + b.Height + b.Height / 2;
            c.RotateAngle = 135;
            c.Left = b.Left + b.Height - a.Height / 4;
            c.Origin = new Point(0, 0);
        }

        public void OnColorChanged(int color)
        {
            var selectedDevices = new List<DeviceViewModel>();
            if (AllBottom)
            {
                selectedDevices.AddRange(BottomDevices);
            }
            if (AllTop)
            {
                selectedDevices.AddRange(TopDevices);
            }
            if (AllDriver)
            {
                selectedDevices.AddRange(DriverDevices);
            }
            if (AllPassenger)
            {
                selectedDevices.AddRange(PassengerDevices);
            }

            foreach (var device in selectedDevices)
            {
                device.SetColor(color);
            }

            AllBottom = AllTop = AllDriver = AllPassenger = false;
            OnPropertyChanged(nameof(AllBottom));
            OnPropertyChanged(nameof(AllTop));
            OnPropertyChanged(nameof(AllDriver));
            OnPropertyChanged(nameof(AllPassenger));
        }
    }
}