using System;
using System.IO;
using Common.Helpers;
using Newtonsoft.Json;

namespace DAndRElectronics.ButtonViewModels
{
    public class AnalogButtonViewModel : ButtonViewModel
    {
        public AnalogButtonViewModel(string buttonName, int col, int row) : base(buttonName, col, row)
        {
            CheckButtonName(buttonName, Constants.AnalogBaseName);
            
            OffColorVisible = false;
            OnColorVisible = false;
            NameVisible = false;
            VoltageVisible = true;
            PatternVisible = false;
        }

        public override bool EquipmentTypeVisible => false;

        public override ButtonViewModel Deserialize(string content)
        {
            return JsonConvert.DeserializeObject<AnalogButtonViewModel>(content);
        }

        protected override void SerializeColors(BinaryWriter writer)
        {
            writer.Write(GetVoltageValue());
            WriteFiveBytes(writer);
        }
        protected override void DeserializeColors(BinaryReader reader)
        {
            var b = reader.ReadByte();
            if (b >= 128)
            {
                _voltageGreaterThan = true;
                Voltage = b - 128;
            }
            else
            {
                Voltage = b;
            }
            
            ReadFiveBytes(reader);
        }

        private byte GetVoltageValue()
        {
            if (_voltageGreaterThan)
            {
                return (byte) (128 + Voltage);
            }

            return (byte) Voltage;
        }
    }
}