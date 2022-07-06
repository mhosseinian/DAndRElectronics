using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Common.Converters;
using Common.Helpers;

namespace PatternBuilderLib.ViewModels
{
    /// <summary>
    /// Interaction logic for DeviceView.xaml
    /// </summary>
    public partial class DeviceView : UserControl
    {
        public DeviceView()
        {
            InitializeComponent();
        }
        

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button button))
            {
                return;
            }
            DropDownPopup.IsOpen = true;
        }

        private void UIElement_OnMouseLeave(object sender, MouseEventArgs e)
        {
        }

        private void MainButton_OnLostFocus(object sender, RoutedEventArgs e)
        {
        }

        private void ColorBtn_OnPreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var btn = sender as Button;
            if (btn == null)
            {
                return;
            }

            var brush = PickBrush();
            if (brush == null)
            {
                return;
            }

            btn.Background = brush;
            var color = ((SolidColorBrush)btn.Background).Color;
            var colorInt = color.R << 16 | color.G << 8 | color.B;
            btn.Command.Execute(colorInt);
            Device.Fill = btn.Background;
            DropDownPopup.IsOpen = false;
        }

        private SolidColorBrush? PickBrush()
        {
            using (new DisposableToken(() => DropDownPopup.StaysOpen = true, () => DropDownPopup.StaysOpen = false,
                       true))
            {
                System.Windows.Forms.ColorDialog colorDialog = new System.Windows.Forms.ColorDialog();
                colorDialog.SolidColorOnly = true;
                if (colorDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var color = Color.FromRgb(colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B);
                    var brush = new SolidColorBrush(color);
                    return brush;
                }
            }

            return null;
        }


    }
}
