using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media;
using Common;

namespace PatternBuilderLib.ViewModels.OutPattern
{
    public class PreviewSingleOutPatternViewModel : ViewModel
    {
        #region Contructors

        public PreviewSingleOutPatternViewModel(byte value)
        {
            Value = value;
        }

        #endregion
        public Byte Value { get; set; }
        private Color _color
        {
            get
            {
                if (Value == 0 || Value == 127)
                {
                    return Colors.Transparent;
                }
                byte r = 255;
                byte g = 0;
                byte b = 128;
                b -= Value;
                return Color.FromRgb(r, g, b);
            }
        }

        public Brush LightColor => new SolidColorBrush(_color);
    }
    public class PreviewOutPatternModelViewModel : ViewModel
    {
        #region Contructors

        public PreviewOutPatternModelViewModel(OutPatternModelViewModel vm)
        {
            ViewModels = new List<PreviewSingleOutPatternViewModel>();
            foreach (var singleOutPatternViewModel in vm.ViewModels)
            {
                ViewModels.Add(new PreviewSingleOutPatternViewModel(singleOutPatternViewModel.Value));
            }
        }

        #endregion

        public List<PreviewSingleOutPatternViewModel> ViewModels { get; set; }
    }
}