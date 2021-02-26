using System;
using System.Windows.Input;
using Common;
using Common.Helpers;
using Newtonsoft.Json;

namespace PatternEditor.ViewModels
{
    public class DeviceViewModel : ViewModel
    {
        
        #region Json properties

        [JsonProperty(PropertyName = Constants.JsonNumDeviceColors)]
        public int NColors { get; set; } = 1;

        [JsonProperty(PropertyName = Constants.JsonColorR)]
        private byte _r;
        [JsonProperty(PropertyName = Constants.JsonColorG)]
        private byte _g;
        [JsonProperty(PropertyName = Constants.JsonColorB)]
        private byte _b;

        #endregion

        [JsonIgnore]
        public int Color
        {
            get => _r << 16 | _g << 8 | _b;
            set
            {
                byte[] values = BitConverter.GetBytes(value);
                if (!BitConverter.IsLittleEndian) Array.Reverse(values);
                _b = values[0];
                _g = values[1];
               _r = values[2];
              OnPropertyChanged();
            }
        }

        [JsonIgnore] public ICommand ColorCommand { get; }
        [JsonIgnore] public double RotateAngle { get; set; } = 0;
        [JsonIgnore] public double Left { get; set; } = 0;
        [JsonIgnore] public double Top { get; set; } = 0;
        [JsonIgnore] public double Width { get; set; } = 60;
        [JsonIgnore] public double Height { get; set; } = 25;
        [JsonIgnore] public double CenterX => Left + (Left + Width)/2;
        [JsonIgnore] public double CenterY => Top + (Top + Height)/2;
        [JsonIgnore] public int Index { get; set; }

        #region Contructors

        public DeviceViewModel()
        {
            ColorCommand = new RelayCommand(OnColor);
        }

        private void OnColor(object obj)
        {
            Color = Convert.ToInt32(obj);
        }

        #endregion

    }
}