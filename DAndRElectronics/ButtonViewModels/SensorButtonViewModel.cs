using System;
using System.IO;
using Common.Helpers;
using Newtonsoft.Json;

namespace DAndRElectronics.ButtonViewModels
{
    public class SensorButtonViewModel : ButtonViewModel
    {
        public SensorButtonViewModel(string buttonName, int col, int row) : base(buttonName, col, row)
        {
            CheckButtonName(buttonName, Constants.SensorBaseName);
           
            OffColorVisible = false;
            OnColorVisible = false;
            NameVisible = false;
            VoltageVisible = false;
            PatternVisible = false;
            PriorityVisible = false;
            SensorVisible = true;
            OutTabVisible = true;
        }

        public override bool CanCopyFrom => false;

        public override bool EquipmentTypeVisible => false;

        public override ButtonViewModel Deserialize(string content)
        {
            return JsonConvert.DeserializeObject<SensorButtonViewModel>(content);
        }

        protected override void SerializeColors(BinaryWriter writer)
        {
            writer.Write((byte)GValue);
            WriteFiveBytes(writer);
        }
        protected override void DeserializeColors(BinaryReader reader)
        {
            GValue = reader.ReadByte();
            ReadFiveBytes(reader);
        }
    }
}