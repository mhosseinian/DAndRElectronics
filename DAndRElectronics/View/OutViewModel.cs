using System;

namespace DAndRElectronics.View
{
    public enum OutTypes
    {
        On,
        Off
    }

    public class OutViewModel : ViewModel
    {
        private Func<int, bool> _getter;
        private Action<int, bool> _setter;
        private readonly int _index;
        #region Contructors

        public OutViewModel(int index, Func<int, bool> getter, Action<int, bool> setter)
        {
            _index = index;
            _getter = getter;
            _setter = setter;
        }

        #endregion
       
        public bool On
        {
            get => _getter(_index);
            set
            {
                _setter(_index, value);
                OnPropertyChanged();
            }
        }

        public string Name => $"Out{_index + 1}";
    }
}