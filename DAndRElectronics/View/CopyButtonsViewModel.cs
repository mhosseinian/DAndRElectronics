using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Common;
using Common.Helpers;
using Common.Services;
using DAndRElectronics.ButtonViewModels;

namespace DAndRElectronics.View
{
    public class CopyButtonViewModel
    {
        private IButton _button;

        #region Contructors

        public CopyButtonViewModel(IButton btn)
        {
            _button = btn;
        }

        #endregion
        public string Name => Button.DisplayButtonName;
        public bool IsSelected { get; set; }

        public IButton Button => _button;
    }
    public class CopyButtonsViewModel: ViewModel
    {
        private IButtonService buttonService;
        private ButtonViewModel _sourceButton;
        #region Contructors

        public CopyButtonsViewModel(object selectedBtn)
        {
            _sourceButton = selectedBtn as ButtonViewModel;
            ApplyCommand = new RelayCommand(OnApply, CanExecute);
            buttonService = ServiceDirectory.Instance.ButtonService;
            var possibleButtons = new List<CopyButtonViewModel>();
            foreach (var button in buttonService.Buttons(selectedBtn))
            {
                var vm = new CopyButtonViewModel(button);
                possibleButtons.Add(vm);
            }

            PossibleButtons = possibleButtons;
        }

        private void OnApply(object obj)
        {
            foreach (var copyButtonViewModel in PossibleButtons.Where(b=> b.IsSelected))
            {
                _sourceButton.CopyTo(copyButtonViewModel.Button);
            }
            var editorService = ServiceDirectory.Instance.GetService<IEditorService>();
            editorService.Close();
        }

        private bool CanExecute(object obj)
        {
            return true;
        }

        #endregion

        public ICommand ApplyCommand { get; set; }
        public IEnumerable<CopyButtonViewModel> PossibleButtons { get; }
    }
}