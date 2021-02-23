using DAndRElectronics.Helpers;
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
            PercentsVisible = false;
        }

        public override bool EquipmentTypeVisible => false;

        public override ButtonViewModel Deserialize(string content)
        {
            return JsonConvert.DeserializeObject<SensorButtonViewModel>(content);
        }
    }
}