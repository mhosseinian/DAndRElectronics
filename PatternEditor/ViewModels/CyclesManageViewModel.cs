using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        private bool _isPreview;

        public ObservableCollection<DeviceManagerViewModel> Cycles { get; set; }

        
        public string PreviewText => "Preview";

        public DeviceManagerViewModel SelectedPreviewItem { get; set; }

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
        public ICommand CloneCommand { get; set; }
        public ICommand PreviewCommand { get; set; }

        public bool IsPreview
        {
            get => _isPreview;
            set
            {
                _isPreview = value;
                OnPropertyChanged();
            }
        }

        #region Contructors

        public CyclesManageViewModel()
        {
            _numDevices = 14;
            Cycles = new ObservableCollection<DeviceManagerViewModel>();
            Cycles.Add(new DeviceManagerViewModel { NumDevices = _numDevices, CycleNumber = 1});
            SelectedItem = Cycles.First();
            AddCommand = new RelayCommand(OnAddItem);
            DeleteCommand = new RelayCommand(OnDeleteItem);
            CloneCommand = new RelayCommand(OnCloneItem);
            PreviewCommand = new RelayCommand(OnPreview); 
        }

        #endregion

        private void OnPreview(object obj)
        {
            IsPreview = true;
            OnPropertyChanged(nameof(PreviewText));
            Task.Factory.StartNew(DoPreview);
        }

        private void DoPreview()
        {
            while (IsPreview)
            {
                foreach (var vm in Cycles)
                {
                    SelectedPreviewItem = vm;
                    OnPropertyChanged(nameof(SelectedPreviewItem));
                    Thread.Sleep(vm.Delay * 10);
                }
            }
        }

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

        private void OnCloneItem(object obj)
        {
            var vm = obj as DeviceManagerViewModel;
            if (vm == null)
            {
                return;
            }

            var item = new DeviceManagerViewModel(vm) { NumDevices = _numDevices, CycleNumber = Cycles.Count + 1 };
            Cycles.Add(item);
            SelectedItem = item;

        }
    }
}