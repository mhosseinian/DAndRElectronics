using System.Collections.Generic;
using System.Windows.Media;
using Common;
using Common.Helpers;
using PatternBuilderLib.Models;

namespace PatternBuilderLib.ViewModels.OutPattern
{
    public class SingleOutPatternViewModel : ViewModel
    {
        public enum OutStatus
        {
            NotUsed = 0,
            Off=1,
            On = 2
        }

        private OutPatternModel _model;
        private int _index;
        public string _label;
        private byte _olderValue;
        #region Contructors

        public SingleOutPatternViewModel( int index, OutPatternModel model)
        {
            _model = model;
            _index = index;
            _olderValue = Value;
            _label = $"{_index + 1}";
            if (_index == 21)
            {
                _label = "ppp";
            }
        }

        #endregion

        public IEnumerable<byte> PossibleValues => Constants.RangedByteEnumeration(5, 100, 5);

        public string Label => _label;
       

        public byte Value
        {
            get => _model.Outs[_index];
            set
            {
                _model.Outs[_index] = value;
                if (value >= 5 && value <= 100)
                {
                    _olderValue = value;
                }
            }
        }

        private bool _isInPreview;

        public bool IsInPreview
        {
            get => _isInPreview;
            set
            {
                _isInPreview = value;
                OnPropertyChanged(nameof(CanShowLight));
            }
        }

        public bool CanShowLight => IsInPreview && IsOn;

        private Color _color
        {
            get
            {
                byte r = 255;
                byte g = 0;
                byte b = 128;
                b -= Value;
                return Color.FromRgb(r,g,b);
            }
        }

        public Brush LightColor => new SolidColorBrush(_color);

        public bool IsOff
        {
            get => Value == 0;
            set
            {
                if (value)
                {
                    Value = 0;
                    NotifyUpdate();
                }
            }
        }

        public bool IsOn
        {
            get=> Value > 0 && Value < 127;
            set
            {
                if (value)
                {
                    if (_olderValue >= 5 && _olderValue <= 100)
                    {
                        Value = _olderValue;
                    }
                    else
                    {
                        Value = 5;
                    }
                    NotifyUpdate();
                }
            }
        }

        public bool NotUsed
        {
            get => Value == 127;
            set
            {
                if (value)
                {
                    Value = 127;
                    NotifyUpdate();
                }
            }
        }


        private void NotifyUpdate()
        {
            OnPropertyChanged(nameof(IsOn));
            OnPropertyChanged(nameof(IsOff));
            OnPropertyChanged(nameof(NotUsed));
            OnPropertyChanged(nameof(Value));
            OnPropertyChanged(nameof(LightColor));
        }
    }
}