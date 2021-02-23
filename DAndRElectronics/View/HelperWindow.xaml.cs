using System;
using System.Windows;
using System.Windows.Input;

namespace DAndRElectronics.View
{
    /// <summary>
    /// Interaction logic for HelperWindow.xaml
    /// </summary>
    public partial class HelperWindow : Window
    {
        public HelperWindow()
        {
            InitializeComponent();
            StateChanged += MainWindowStateChangeRaised;
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            // Content of window may be black in case of SizeToContent is set. 
            // This eliminates the problem. 
            // Do not use InvalidateVisual because it may implicitly break your markup.
            InvalidateMeasure();
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
            //if (WindowState == WindowState.Maximized)
            //{
            //    MainWindowBorder.BorderThickness = new Thickness(8);
            //    RestoreButton.Visibility = Visibility.Visible;
            //    MaximizeButton.Visibility = Visibility.Collapsed;
            //}
            //else
            //{
            //    MainWindowBorder.BorderThickness = new Thickness(0);
            //    RestoreButton.Visibility = Visibility.Collapsed;
            //    MaximizeButton.Visibility = Visibility.Visible;
            //}
        }

        public void SetContent(FrameworkElement content)
        {
            AppArea.Children.Clear();
            content.HorizontalAlignment = HorizontalAlignment.Stretch;
            content.VerticalAlignment = VerticalAlignment.Stretch;
            AppArea.Children.Add(content);
        }

       
    }
}
