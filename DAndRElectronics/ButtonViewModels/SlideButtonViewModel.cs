using System.Collections.Generic;
using System.Reflection.Metadata;
using DAndRElectronics.Helpers;
using Newtonsoft.Json;

namespace DAndRElectronics.ButtonViewModels
{
    public class SlideButtonViewModel : ButtonViewModel
    {
        public SlideButtonViewModel(string buttonName, int col, int row) : base(buttonName, col, row)
        {
            CheckButtonName(buttonName, Constants.SlideBaseName);
            PercentsVisible = false;
        }

        public override ButtonViewModel Deserialize(string content)
        {
            return JsonConvert.DeserializeObject<SlideButtonViewModel>(content);
        }

        public override IEnumerable<string> PossibleTypes => Constants.PossibleTypesSlider;
    }
}