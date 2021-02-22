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
using DAndRElectronics.Services;

namespace DAndRElectronics.View
{
    /// <summary>
    /// Interaction logic for KeyboardView.xaml
    /// </summary>
    public partial class KeyboardView : UserControl
    {
        public KeyboardView()
        {
            InitializeComponent();
            var stateService = ServiceDirectory.Instance.GetService<IStateService>();
            stateService.Subscribe(this,OnStateChanged);
            this.DataContextChanged +=OnDataContextChanged;
        }

        private void OnStateChanged(StateChangedTypes obj)
        {
            PopulateGrids();
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            PopulateGrids();
        }

        private void PopulateGrids()
        {
            GridKeys.Children.Clear();
            GridInputs.Children.Clear();
            GridEvents.Children.Clear();
            GridSlide.Children.Clear();
            GridAnalog.Children.Clear();
            GridTimer.Children.Clear();
            var vm = DataContext as KeyboardViewModel;
            
            SetupGrid(GridKeys, vm.KeyButtons);
            SetupGrid(GridInputs, vm.InputButtons);
            SetupGrid(GridEvents, vm.EventButtons);
            SetupGrid(GridSlide, vm.SlideButtons);
            SetupGrid(GridAnalog, vm.AnalogButtons);
            SetupGrid(GridTimer, vm.TimerButtons);
        }

        private void SetupGrid(Grid grid, IEnumerable<ButtonViewModel> buttons)
        {
            foreach (var buttonVm in buttons)
            {
                var buttonView = new CustomButton { DataContext = buttonVm };
                grid.Children.Add(buttonView);
                Grid.SetColumn(buttonView, buttonVm.Column);
                Grid.SetRow(buttonView, buttonVm.Row);
            }
        }
    }
}
