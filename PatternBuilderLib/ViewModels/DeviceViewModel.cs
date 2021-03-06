﻿using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Input;
using Common;
using Common.Helpers;
using Common.Services;
using Newtonsoft.Json;

namespace PatternBuilderLib.ViewModels
{
    [Serializable]
    public class DeviceViewModel : ViewModel
    {
        private static int dummy = 1;
        #region Json properties

        [JsonProperty(PropertyName = Constants.JsonNumDeviceColors)]
        public string Name { get; set; }
        public int NColors { get; set; } = 1;

        [JsonProperty(PropertyName = Constants.JsonColorR)]
        private byte _r;
        [JsonProperty(PropertyName = Constants.JsonColorG)]
        private byte _g;
        [JsonProperty(PropertyName = Constants.JsonColorB)]
        private byte _b;

        #endregion

        public void SerializeBinary(BinaryWriter binWriter)
        {
            binWriter.Write(_r);
            binWriter.Write(_g);
            binWriter.Write(_b);
        }

        [JsonIgnore]
        public int Color
        {
            get => _r << 16 | _g << 8 | _b;
            set
            {
                SetColor(value);
                var service = ServiceDirectory.Instance.GetService<IButtonSelectionService>();
                service.ButtonColorChanged(value);
            }
        }

        public void SetColor(int color)
        {
            byte[] values = BitConverter.GetBytes(color);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(values);
            }
            _b = values[0];
            _g = values[1];
            _r = values[2];
              OnPropertyChanged(nameof(Color));
        }


        [IgnoreDataMember] [JsonIgnore] public ICommand ColorCommand { get; set; }
        [IgnoreDataMember] [JsonIgnore] public ICommand ClickedCommand { get; set; }
        [JsonIgnore] public double RotateAngle { get; set; } = 0;
        [JsonIgnore] public System.Windows.Point Origin { get; set; } = new Point(0,0);
        [JsonIgnore] public double Left { get; set; } = 0;
        [JsonIgnore] public double Top { get; set; } = 0;
        [JsonIgnore] public double Width { get; set; } = 60;
        [JsonIgnore] public double Height { get; set; } = 25;
        [JsonIgnore] public double CenterX => Left + (Left + Width)/2;
        [JsonIgnore] public double CenterY => Top + (Top + Height)/2;
        [JsonIgnore] public int Index { get; set; }
        [JsonIgnore] public bool ColorBoxOpen { get; set; }

        #region Contructors

        public DeviceViewModel()
        {
            Name = $"{dummy}";
            dummy++;
            AssignCommands();
        }
       

        private void OnColor(object obj)
        {
            Color = Convert.ToInt32(obj);
            ColorBoxOpen = false;
            OnPropertyChanged(nameof(ColorBoxOpen));
        }
        private void OnClicked(object obj)
        {
            ColorBoxOpen = true;
            OnPropertyChanged(nameof(ColorBoxOpen));
        }

        #endregion

        public void AssignCommands()
        {
            ColorCommand = new RelayCommand(OnColor);
            ClickedCommand = new RelayCommand(OnClicked);
        }

        public static MemoryStream SerializeToStream(object o)
        {
            MemoryStream stream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, o);
            return stream;
        }

        public static object DeserializeFromStream(MemoryStream stream)
        {
            IFormatter formatter = new BinaryFormatter();
            stream.Seek(0, SeekOrigin.Begin);
            object o = formatter.Deserialize(stream);
            return o;
        }

        public static DeviceViewModel Clone(DeviceViewModel src)
        {
            MemoryStream stream = SerializeToStream(src);

            var ret  = (DeviceViewModel)DeserializeFromStream(stream);
            ret.AssignCommands();
            return ret;
        }

    }
}