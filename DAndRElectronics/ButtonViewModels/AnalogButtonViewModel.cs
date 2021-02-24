using System;
using DAndRElectronics.Annotations;
using DAndRElectronics.Helpers;
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
    }
}