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
            SetupButtons();
        }

        public void SetupButtons()
        {
            if (!(DataContext is DeviceManagerViewModel vm))
            {
                return;
            }

            if (vm.IsLine)
            {
                DockControl.Children.Clear();
                foreach (var deviceViewModel in vm.Devices)
                {
                    var child = new DeviceView {DataContext = deviceViewModel};
                    DockControl.Children.Add(child);
                    DockPanel.SetDock(child, Dock.Left);
                }
            }
            else
            {
                CanvasControl.Children.Clear();
                foreach (var deviceViewModel in vm.Devices)
                {
                    var child = new DeviceView {DataContext = deviceViewModel};
                    CanvasControl.Children.Add(child);
                    Canvas.SetTop(child, deviceViewModel.Top);
                    Canvas.SetLeft(child, deviceViewModel.Left);
                }
            }
        }
    }
}
