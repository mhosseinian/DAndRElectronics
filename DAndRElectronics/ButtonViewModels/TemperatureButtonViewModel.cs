using DAndRElectronics.Helpers;
using Newtonsoft.Json;

namespace DAndRElectronics.ButtonViewModels
{
    public class TemperatureButtonViewModel : ButtonViewModel
    {
        public TemperatureButtonViewModel(string buttonName, int col, int row) : base(buttonName, col, row)
        {
            CheckButtonName(buttonName, Constants.TemperatureBaseName);
            OffColorVisible = false;
            OnColorVisible = false;
            NameVisible = false;
            PatternVisible = false;
            VoltageVisible = false;
            TimerVisible = false;
            PriorityVisible = false;
            TemperatureVisible = true;
        }

        public override ButtonViewModel Deserialize(string content)
        {
            return JsonConvert.DeserializeObject<TemperatureButtonViewModel>(content);
        }

        public override bool EquipmentTypeVisible => false;
    }
}