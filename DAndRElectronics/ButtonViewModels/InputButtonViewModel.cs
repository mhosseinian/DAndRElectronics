using System.IO;
using Common.Helpers;
using Newtonsoft.Json;

namespace DAndRElectronics.ButtonViewModels
{
    public class InputButtonViewModel : ButtonViewModel
    {
        private const string HornRing = "Horn ring";
        public InputButtonViewModel(string buttonName, int col, int row) : base(buttonName, col, row)
        {
            CheckButtonName(buttonName, Constants.InputBaseName);
            OffColorVisible = false;
            OnColorVisible = false;
            NameVisible = false;
        }

        public override ButtonViewModel Deserialize(string content)
        {
            return JsonConvert.DeserializeObject<InputButtonViewModel>(content);
        }

        public override bool EquipmentTypeVisible => false;

        public override string DisplayButtonName => ButtonName.EndsWith("17") ? HornRing : base.DisplayButtonName;

        protected override void SpecialHandlingAtEnd(BinaryWriter writer)
        {
            if (DisplayButtonName == HornRing)
            {
                writer.Seek(6, SeekOrigin.Current);
            }
        }
    }
}