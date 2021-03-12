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
            get
            {
                if (_index >= ButtonViewModel.MaxKeys)
                {
                    return Constants.OFF;
                }
                return Constants.OnOffNotUseMappingsReversed[_outputs.OutputKeys[_index]];
            }
            set
            {
                if (_index >= ButtonViewModel.MaxKeys)
                {
                    return;
                }
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

        public bool IsOnOffVisible => _index != ButtonViewModel.MaxOuts - 1;

        public IEnumerable<int> PossibleValues => Constants.RangedEnumeration(0, 100, 5);
        public IEnumerable<string> PossibleKeyValues => Constants.OnOffNotUseMappings.Keys;

        public string Name =>  _index == ButtonViewModel.MaxOuts-1?  "H R L" : $"Out{_index + 1}";
        public string ToolTip =>  _index == ButtonViewModel.MaxOuts-1?  "Horn ring" : $"Out{_index + 1}";

        public string ButtonName => _index == ButtonViewModel.MaxOuts-1?  string.Empty : $"KEY{_index + 1}";

    }
}