using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using DAndRElectronics.ButtonViewModels;
using DAndRElectronics.Enums;
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

        private void OnStateChanged(StateChangedTypes stateType)
        {
            if (stateType == StateChangedTypes.ProjectOpened)
            {
                PopulateGrids();
            }
            else if (stateType == StateChangedTypes.EventButtonAdded)
            {
                var vm = DataContext as KeyboardViewModel;
                SetupGrid(GridEvents, vm.EventButtons);
            }
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            PopulateGrids();
        }

        private void PopulateGrids()
        {
            var vm = DataContext as KeyboardViewModel;
            
            SetupGrid(GridKeys, vm.KeyButtons);
            SetupGrid(GridInputs, vm.InputButtons);
            SetupGrid(GridEvents, vm.EventButtons);
            SetupGrid(GridSlide, vm.SlideButtons);
            SetupGrid(GridAnalog, vm.AnalogButtons);
            SetupGrid(GridTimer, vm.TimerButtons);
            SetupGrid(GridSensor, vm.SensorButtons);
            SetupGrid(GridTemperature, vm.TemperatureButtons);
        }

        private void SetupGrid(Grid grid, IEnumerable<ButtonViewModel> buttons)
        {
            if (buttons == null)
            {
                return;
            }
            grid.Children.Clear();
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
