using System;
using System.IO;
using Common.Helpers;
using Newtonsoft.Json;

namespace DAndRElectronics.ButtonViewModels
{
    public class TimerButtonViewModel : ButtonViewModel
    {
        public TimerButtonViewModel(string buttonName, int col, int row) : base(buttonName, col, row)
        {
            CheckButtonName(buttonName, Constants.TimerBaseName);

            OffColorVisible = false;
            OnColorVisible = false;
            NameVisible = false;
            PatternVisible = false;
            PriorityVisible = false;
            VoltageVisible = false;
            TimerVisible = true;
            TemperatureVisible = false;
            SyncVisible = false;
            OutTabVisible = false;
        }

        public override void Serialize(BinaryWriter writer)
        {
            writer.Seek(570, SeekOrigin.Begin);
            writer.Write((byte)Timer);
        }

      

        public override ButtonViewModel Deserialize(string content)
        {
            return JsonConvert.DeserializeObject<TimerButtonViewModel>(content);
        }

        protected override void SpecialHandlingAtBeginning(BinaryWriter writer)
        {
            writer.Write((byte)Timer);
        }

        public override bool EquipmentTypeVisible => false;

        protected override void SerializeColors(BinaryWriter writer)
        {
            writer.Write((byte)0);
            WriteFiveBytes(writer);
        }

        protected override void DeserializeColors(BinaryReader reader)
        {
            Timer = reader.ReadByte();
            ReadFiveBytes(reader);
        }
    }
}