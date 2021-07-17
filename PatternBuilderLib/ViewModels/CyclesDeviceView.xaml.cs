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

namespace PatternBuilderLib.ViewModels
{
    /// <summary>
    /// Interaction logic for CyclesDeviceView.xaml
    /// </summary>
    public partial class CyclesDeviceView : UserControl
    {
        public CyclesDeviceView()
        {
            InitializeComponent();
        }

        //private void OnFlatOvalButtonClicked(object sender, RoutedEventArgs e)
        //{
        //    if (!(DataContext is CyclesManageViewModel vm))
        //    {
        //        return;
        //    }

        //    ViewModels.DeviceManagerView.SetupButtons();
        //}

        //private void WidthSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        //{
        //    if (!(DataContext is CyclesManageViewModel vm))
        //    {
        //        return;
        //    }
        //    ViewModels.DeviceManagerView.SetupButtons();
        //}
    }
}
