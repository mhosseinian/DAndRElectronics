using System.Collections.Generic;
using Common;
using Common.Helpers;
using DAndRElectronics.ButtonViewModels;

namespace DAndRElectronics.View
{
    public class OutViewModel : ViewModel
    {
        
        private readonly int _index;
        private readonly IOutputs _outputs;

        #region Contructors

        public OutViewModel(int index, IOutputs outputs)
        {
            _outputs = outputs;
            _index = index;
        }

        #endregion

        public bool On
        {
            get => _outputs.Outputs[_index];
            set
            {
                _outputs.Outputs[_index] = value;
                OnPropertyChanged();
            }
        }
        public string KeyOn
        {
            get => Constants.OnOffNotUseMappingsReversed[_outputs.OutputKeys[_index]] ;
            set
            {
                _outputs.OutputKeys[_index] = Constants.OnOffNotUseMappings[value];
                OnPropertyChanged();
            }
        }

        public int Percent
        {
            get => _outputs.OutputPercents[_index];
            set
            {
                _outputs.OutputPercents[_index] = value;
                OnPropertyChanged();
            }
        }

        public IEnumerable<int> PossibleValues => Constants.RangedEnumeration(0, 100, 5);
        public IEnumerable<string> PossibleKeyValues => Constants.OnOffNotUseMappings.Keys;

        public string Name => $"Out{_index + 1}";

        public string ButtonName => $"KEY{_index + 1}";

    }
}