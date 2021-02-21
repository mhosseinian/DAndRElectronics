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
            var vm = DataContext as KeyboardViewModel;
            foreach (var buttonVm in vm.KeyButtons)
            {
                var buttonView = new CustomButton {DataContext = buttonVm};
                GridKeys.Children.Add(buttonView);
                Grid.SetColumn(buttonView, buttonVm.Column);
                Grid.SetRow(buttonView, buttonVm.Row);
            }

            foreach (var buttonVm in vm.InputButtons)
            {
                var buttonView = new CustomButton {DataContext = buttonVm};
                GridInputs.Children.Add(buttonView);
                Grid.SetColumn(buttonView, buttonVm.Column);
                Grid.SetRow(buttonView, buttonVm.Row);
            }

            foreach (var buttonVm in vm.EventButtons)
            {
                var buttonView = new CustomButton {DataContext = buttonVm};
                GridEvents.Children.Add(buttonView);
                Grid.SetColumn(buttonView, buttonVm.Column);
                Grid.SetRow(buttonView, buttonVm.Row);
            }
        }
    }
}
