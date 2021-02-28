using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Helpers;
using Newtonsoft.Json;

namespace PatternBuilderLib.ViewModels
{
    public class DeviceManagerViewModel: ViewModel
    {
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
                    PopulateDevices();
                }
                OnPropertyChanged();
            }
        }

        public string DeleteText => $"Delete Cycle {CycleNumber}";
        [JsonIgnore] public string CloneText => $"Clone Cycle {CycleNumber}";
        [JsonIgnore] public string Label => $"Cycle # {CycleNumber}";

        public int Delay { get; set; }

        #region Contructors

        public DeviceManagerViewModel()
        {
        }
        public DeviceManagerViewModel(int numCycles, bool isLine)
        {
            IsLine = isLine;
            NumDevices = numCycles;
            for (var i = 0; i < NumDevices; i++)
            {
                Devices.Add(new DeviceViewModel{Index = i+1});
            }

            if (!IsLine)
            {
                PopulateDevices();
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
            
            Delay = src.Delay;
        }

        #endregion

        public void PopulateDevices()
        {
            //Always three on each side
            var numHorizontal = (NumDevices - 6) / 2;


            var left = 100.0;
            for (var i=0; i < numHorizontal; i++)
            {
                Devices[i].Left = left;
                left += Devices[i].Width;
            }

            var rotation = 45;
            //left -= Devices.First().Width;
            var top = Devices.First().Height;
            var counter = numHorizontal;
            Devices[counter].RotateAngle = 90 - rotation;
            Devices[counter].Left = left;
            Devices[counter].Top = top;
            
            counter++;
            
            Devices[counter].RotateAngle = 90;
            Devices[counter].Left = left + Devices[counter].Width/2 - Devices[counter].Width/12;
            Devices[counter].Top = Devices[counter].Width* 1.416;
            
            counter++;
            Devices[counter].RotateAngle = 90 + rotation;
            Devices[counter].Left = left;
            Devices[counter].Top = (2* Devices[counter].Width) + Devices[counter].Height;

            //Driver side
            var startLeftSide= (numHorizontal*2) + 3;
            Devices[startLeftSide].RotateAngle = -135;
            Devices[startLeftSide].Top = Devices[counter].Width * 2.416;
            Devices[startLeftSide].Left = Devices[counter].Width*0.666;

            startLeftSide++;
            Devices[startLeftSide].RotateAngle = 90;
            Devices[startLeftSide].Top = Devices[counter].Width* 1.4166;
            Devices[startLeftSide].Left = Devices[counter].Width * 0.25;
            startLeftSide++;
            Devices[startLeftSide].RotateAngle = 135;
            Devices[startLeftSide].Top = Devices[counter].Width * 0.3666;
            Devices[startLeftSide].Left = Devices[counter].Width* 0.6666;


            var start = (numHorizontal*2) + 2;
            var endLoop = start - 3;
            left = 100.0;
            for (var i = start; i >= endLoop; i--)
            {
                Devices[i].Left = left;
                left += Devices[i].Width;
                Devices[i].Top = Devices[counter].Width* 2.8333;
            }

        }

    }
}