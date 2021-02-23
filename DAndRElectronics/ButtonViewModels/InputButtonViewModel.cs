using DAndRElectronics.Helpers;
using Newtonsoft.Json;

namespace DAndRElectronics.ButtonViewModels
{
    public class InputButtonViewModel : ButtonViewModel
    {
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
    }
}