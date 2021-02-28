using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Helpers;
using Newtonsoft.Json;

namespace PatternBuilderLib.ViewModels
{
    public class DeviceManagerViewModel: ViewModel
    {
        private const double StartPosition = 100.0;

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


            var left = StartPosition;
            for (var i=0; i < numHorizontal; i++)
            {
                Devices[i].Left = left;
                left += Devices[i].Width;
            }

            //Driver side
            var startLeftSide= (numHorizontal*2) + 3;

            var leftSidButtons = new[]{Devices[startLeftSide], Devices[startLeftSide+1], Devices[startLeftSide+2]};
            var rightSidButtons = new[]{Devices[numHorizontal+2], Devices[numHorizontal + 1], Devices[numHorizontal]};
            SetSideButtons(leftSidButtons, rightSidButtons, numHorizontal);
           

            var start = (numHorizontal*2) + 2;
            var endLoop = start - numHorizontal + 1;
            left = StartPosition;
            for (var i = start; i >= endLoop; i--)
            {
                Devices[i].Left = left;
                left += Devices[i].Width;
                Devices[i].Top = Devices[0].Width* 2.8333;
            }

        }

        private static void SetSideButtons(DeviceViewModel[] leftsideButtons, DeviceViewModel[] rightsideButtons, int numHorizontalLights)
        {
            var width = leftsideButtons.First().Width;
            
            var counter = 0;
            leftsideButtons[counter].RotateAngle = -135;
            leftsideButtons[counter].Top =  leftsideButtons[counter].Width * 2.416;
            leftsideButtons[counter].Left = leftsideButtons[counter].Width * 0.666;

            counter++;
            leftsideButtons[counter].RotateAngle = 90;
            leftsideButtons[counter].Top =  leftsideButtons[counter].Width * 1.4166;
            leftsideButtons[counter].Left = leftsideButtons[counter].Width * 0.25;
            counter++;
            leftsideButtons[counter].RotateAngle = 135;
            leftsideButtons[counter].Top =  leftsideButtons[counter].Width * 0.3666;
            leftsideButtons[counter].Left = leftsideButtons[counter].Width * 0.6666;

            counter = 0;
            rightsideButtons[counter].RotateAngle = -45;
            rightsideButtons[counter].Top = leftsideButtons[counter].Top;
            rightsideButtons[counter].Left = leftsideButtons[counter].Left + (numHorizontalLights + 1) * (width);

            counter++;
            rightsideButtons[counter].RotateAngle = 90;
            rightsideButtons[counter].Top = leftsideButtons[counter].Top;
            var diff = StartPosition - leftsideButtons[counter].Left;
            rightsideButtons[counter].Left = leftsideButtons[counter].Left + (numHorizontalLights)*(width) + diff + width/2.5;

            counter++;
            rightsideButtons[counter].RotateAngle = -135;
            rightsideButtons[counter].Top = leftsideButtons[counter].Top;
            rightsideButtons[counter].Left = leftsideButtons[counter].Left + (numHorizontalLights + 1) * (width);
        }

    }
}