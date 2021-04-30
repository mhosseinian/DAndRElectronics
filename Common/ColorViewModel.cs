using System;
using System.IO;
using System.Windows.Media;

namespace Common
{
    public class ColorViewModel : ViewModel
    {
        #region Private fields

        private byte _r;
        private byte _g;
        private byte _b;
        private Color _color;
       

        private int _intColor;

        #endregion


        #region Public properties

        public int IntColor
        {
            get => R << 16 | G << 8 | B;
            set => SetColors(value);
        }


        public Color Color
        {
            get => Color.FromRgb(R, G, B);
            set
            {
                R = value.R;
                G = value.G;
                B = value.B;
            }
        }

        public byte R
        {
            get => _r;
            set
            {
                _r = value; OnPropertyChanged();OnPropertyChanged(nameof(Color));
            }
        }

        public byte G
        {
            get => _g;
            set { _g = value; OnPropertyChanged(); OnPropertyChanged(nameof(Color)); }
        }

        public byte B
        {
            get => _b;
            set { _b = value; OnPropertyChanged(); OnPropertyChanged(nameof(Color)); }
        }

        #endregion


        #region Contructors

        public ColorViewModel(int intColor)
        {
            IntColor = intColor;
        }
        public ColorViewModel(Color color)
        {
            Color = color;
        }

        public ColorViewModel(BinaryReader reader)
        {
            Deserialize(reader);
        }


        #endregion

        #region Serialization

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(R);
            writer.Write(G);
            writer.Write(B);
        }

        public void Deserialize(BinaryReader reader)
        {
            R = reader.ReadByte();
            G = reader.ReadByte();
            B = reader.ReadByte();
        }

        #endregion


        private void SetColors(int color)
        {
            var values = BitConverter.GetBytes(color);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(values);
            }

            B = values[0];
            G = values[1];
            R = values[2];
        }

    }
}