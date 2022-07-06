using System;
using System.Windows;
using System.Windows.Input;
using Common.Helpers;
using Common.Services;
using Microsoft.Extensions.DependencyInjection;
using PatternBuilderLib.ViewModels;


namespace PatternEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public KeyboardViewModel ViewModel { get; set; } = new KeyboardViewModel();
        public MainWindow()
        {

            InitializeComponent();

            if (FeatureAccessManager.FeatureAvailable(FeatureAccessManager.SelectLighModelFeature))
            {
                Loaded += (sender, args) => DisplayModels();
            }
            else
            {
                var view = new CyclesManageView { DataContext = new CyclesManageViewModel(18, true) };
                AppArea.Children.Add(view);
                this.Width = 18 * 50 + 360;
            }

            StateChanged += MainWindowStateChangeRaised;

        }

        private void DisplayModels()
        {
            var vm = new LightManagerViewModel();
            var view = new LightbarManagerView {DataContext = vm};
            var service = ServiceDirectory.Instance.GetService<IEditorService>();
            service.SetContentWithSize(view, "Select a model", null, 400, 300, true);
            var newView = new CyclesManageView { DataContext = new CyclesManageViewModel(vm.SelectedItem.NumLights, vm.IsLine) };
            AppArea.Children.Add(newView);
            this.Width = vm.SelectedItem.NumLights * 50 + 360;
        }

        // Can execute
        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        // Minimize
        private void CommandBinding_Executed_Minimize(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        // Maximize
        private void CommandBinding_Executed_Maximize(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        // Restore
        private void CommandBinding_Executed_Restore(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }

        // Close
        private void CommandBinding_Executed_Close(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }

        private void MainWindowStateChangeRaised(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                MainWindowBorder.BorderThickness = new Thickness(8);
                RestoreButton.Visibility = Visibility.Visible;
                MaximizeButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                MainWindowBorder.BorderThickness = new Thickness(0);
                RestoreButton.Visibility = Visibility.Collapsed;
                MaximizeButton.Visibility = Visibility.Visible;
            }
        }
    }
}
