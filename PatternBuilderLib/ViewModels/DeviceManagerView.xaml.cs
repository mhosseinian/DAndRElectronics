using System.Windows;
using System.Windows.Controls;

namespace PatternBuilderLib.ViewModels
{
    /// <summary>
    /// Interaction logic for DeviceManagerView.xaml
    /// </summary>
    public partial class DeviceManagerView : UserControl
    {
        public DeviceManagerView()
        {
            InitializeComponent();
            DataContextChanged +=OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(!(DataContext is DeviceManagerViewModel vm))
            {
                return;
            }

            //var num = vm.NumDevices;
            //var start = 0;
            //var end = 6;
            //for (var i = start; i <= end; i++)
            //{
            //    var deviceViewModel = vm.Devices[i];
            //    var child = new DeviceView { DataContext = deviceViewModel };
            //    DeviceControl.Children.Add(child);
            //    Canvas.SetTop(child, deviceViewModel.Top);
            //    Canvas.SetLeft(child, deviceViewModel.Left);
            //}


            //start = 7;
            //end = 10;
            //for (var i = start; i <= end; i++)
            //{
            //    var deviceViewModel = vm.Devices[i];
            //    var child = new DeviceView { DataContext = deviceViewModel };
            //    DeviceControl.Children.Add(child);
            //    Canvas.SetTop(child, deviceViewModel.Top);
            //    Canvas.SetLeft(child, deviceViewModel.Left);
            //}
            foreach (var deviceViewModel in vm.Devices)
            {
                var child = new DeviceView { DataContext = deviceViewModel };
                DeviceControl.Children.Add(child);
                Canvas.SetTop(child, deviceViewModel.Top);
                Canvas.SetLeft(child, deviceViewModel.Left);
            }
        }
    }
}
