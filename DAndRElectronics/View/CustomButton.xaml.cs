using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DAndRElectronics.View
{
    /// <summary>
    /// Interaction logic for CustomButton.xaml
    /// </summary>
    public partial class CustomButton : UserControl
    {
        public CustomButton()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var buttonView = new ButtonView(){DataContext = this.DataContext, Height = 600, Width = 600};
            var w = new HelperWindow() { SizeToContent = SizeToContent.WidthAndHeight, Title = (DataContext as ButtonViewModel).ButtonName, Height = 600, Width = 600};
           w.SetContent(buttonView);
            w.ShowDialog();
        }
    }
}
