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

        public override ButtonViewModel Deserialize(string content)
        {
            return JsonConvert.DeserializeObject<TimerButtonViewModel>(content);
        }

        public override bool EquipmentTypeVisible => false;
    }
}