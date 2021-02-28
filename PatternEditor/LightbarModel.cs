using System.Collections.Generic;
using System.Security.Cryptography;
using Common;

namespace PatternEditor
{
    public class LightbarModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumLights { get; set; }

        public string Display => $"{Name}: {Description}";
    }

    public class LightManagerViewModel : ViewModel
    {
        private LightbarModel _selectedItem;
        private List<LightbarModel> _models = new List<LightbarModel>
        {
            new LightbarModel{Name="SL-24", Description="Front 2 / Rear 2", NumLights = 10},
            new LightbarModel{Name="SL-30", Description="Front 3 / Rear 3", NumLights = 12},
            new LightbarModel{Name="SL-36", Description="Front 4 / Rear 4", NumLights = 14},
            new LightbarModel{Name="SL-42", Description="Front 5 / Rear 5", NumLights = 16},
            new LightbarModel{Name="SL-48", Description="Front 6 / Rear 6", NumLights = 18},
            new LightbarModel{Name="SL-54", Description="Front 7 / Rear 7", NumLights = 20},
            new LightbarModel{Name="SL-60", Description="Front 8 / Rear 8", NumLights = 22},
            new LightbarModel{Name="SL-66", Description="Front 9 / Rear 9", NumLights = 24},
            new LightbarModel{Name="SL-72", Description="Front 10 / Rear 10", NumLights = 26},
            new LightbarModel{Name="SL-78", Description="Front 11 / Rear 11", NumLights = 28},
            new LightbarModel{Name="SL-84", Description="Front 12 / Rear 12", NumLights = 30},
            new LightbarModel{Name="SL-90", Description="Front 13 / Rear 13", NumLights = 32},
            new LightbarModel{Name="SL-96", Description="Front 14 / Rear 14", NumLights = 34},
        };

        public IEnumerable<LightbarModel> Models => _models;

        public LightbarModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
            }
        }
    }
}