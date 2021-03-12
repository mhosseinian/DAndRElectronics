using System;
using System.IO;
using System.Windows.Media;

namespace Common.Helpers
{
    public class ColorHelper
    {
        public byte R { get; private set; }
        public byte G { get; private set; }
        public byte B { get; private set; }

        private int _intColor;

        public int IntColor
        {
            get => _intColor;
            set
            {
                _intColor = value;
                SetColors(_intColor);
            }
        }

        private Color _color;

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

        #region Contructors

        public ColorHelper(int intColor)
        {
            IntColor = intColor;
        }
        public ColorHelper(Color color)
        {
            Color = color;
        }

        public ColorHelper(BinaryReader reader)
        {
            Deserialize(reader);
        }


        #endregion

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