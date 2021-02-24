using DAndRElectronics.Helpers;
using Newtonsoft.Json;

namespace DAndRElectronics.ButtonViewModels
{
    public class EventButtonViewModel : ButtonViewModel
    {
        public EventButtonViewModel(string buttonName, int col, int row) : base(buttonName, col, row)
        {
            CheckButtonName(buttonName, Constants.EventBaseName);

            OffColorVisible = false;
            OnColorVisible = false;
            NameVisible = false;
            PatternVisible = false;
            PriorityVisible = false;
            VoltageVisible = false;
            TimerVisible = false;
            TemperatureVisible = false;
            EventVisible = true;
            CanDelete = true;
        }

        public override ButtonViewModel Deserialize(string content)
        {
            return JsonConvert.DeserializeObject<EventButtonViewModel>(content);
        }

        public override bool EquipmentTypeVisible => false;
    }
}