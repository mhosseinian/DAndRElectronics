using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Common;
using Common.Helpers;

namespace PatternEditor.ViewModels
{
    public class CyclesManageViewModel : ViewModel
    {
        private int _numDevices = 1;
        private DeviceManagerViewModel _selectedItem;

        public ObservableCollection<DeviceManagerViewModel> Cycles { get; set; }

        public DeviceManagerViewModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }
        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        #region Contructors

        public CyclesManageViewModel()
        {
            _numDevices = 14;
            Cycles = new ObservableCollection<DeviceManagerViewModel>();
            Cycles.Add(new DeviceManagerViewModel { NumDevices = _numDevices, CycleNumber = 1});
            SelectedItem = Cycles.First();
            AddCommand = new RelayCommand(OnAddItem);
            DeleteCommand = new RelayCommand(OnDeleteItem); 
        }

        #endregion

        private void OnAddItem(object obj)
        {
            var item = new DeviceManagerViewModel {NumDevices = _numDevices, CycleNumber = Cycles .Count+1};
            Cycles.Add(item);
            SelectedItem = item;
        }
        private void OnDeleteItem(object obj)
        {
            var vm = obj as DeviceManagerViewModel;
            if (vm == null)
            {
                return;
            }

            Cycles.Remove(vm);
            if (!Cycles.Any())
            {
                SelectedItem = null;
            }
            else
            {
                SelectedItem = Cycles.Last();
            }
            
        }
    }
}