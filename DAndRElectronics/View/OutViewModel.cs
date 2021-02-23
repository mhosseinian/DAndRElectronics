using System;
using System.Collections.Generic;
using System.Linq;
using DAndRElectronics.ButtonViewModels;
using DAndRElectronics.Helpers;

namespace DAndRElectronics.View
{
    public class OutViewModel : ViewModel
    {
        private const string ON = "On";
        private const string OFF = "Off";
        private const string NOTUSED = "Not used";

        private Dictionary<string,int> _valueMappings = new Dictionary<string, int>
        {
            {OFF, 0},
            {ON, 1},
            {NOTUSED, 2},
        };
        private Dictionary<int,string> _valueMappingsReversed = new Dictionary<int, string>
        {
            {0,OFF},
            {1,ON},
            {2,NOTUSED},
        };
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
            get => _valueMappingsReversed[_outputs.OutputKeys[_index]] ;
            set
            {
                _outputs.OutputKeys[_index] = _valueMappings[value];
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
        public IEnumerable<string> PossibleKeyValues => _valueMappings.Keys;

        public string Name => $"Out{_index + 1}";

        public string ButtonName => $"KEY{_index + 1}";

    }
}