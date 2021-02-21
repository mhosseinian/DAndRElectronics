using System.Collections.Generic;

namespace DAndRElectronics.View
{
    public class KeyboardViewModel:ViewModel
    {
        private List<ButtonViewModel> _buttons = new List<ButtonViewModel>();
        public IEnumerable<ButtonViewModel> Buttons => _buttons;
        

        #region Contructors

        public KeyboardViewModel()
        {
            PopulateKeyButtons();
        }

        private void PopulateKeyButtons()
        {

            var row = 0;
            var col = 0;
            _buttons.Add(new ButtonViewModel("KEY1", col, row));
            _buttons.Add(new ButtonViewModel("KEY2", ++col, row));
            _buttons.Add(new ButtonViewModel("KEY3", ++col, row));
            _buttons.Add(new ButtonViewModel("KEY4", ++col, row));
            _buttons.Add(new ButtonViewModel("KEY5", ++col, row));
            col = 0;
            row++;
            _buttons.Add(new ButtonViewModel("KEY6",  col, row));
            _buttons.Add(new ButtonViewModel("KEY7",  ++col, row));
            _buttons.Add(new ButtonViewModel("KEY8",  ++col, row));
            _buttons.Add(new ButtonViewModel("KEY9",  ++col, row));
            _buttons.Add(new ButtonViewModel("KEY10", ++col, row));
            col = 0;
            row++;
            _buttons.Add(new ButtonViewModel("KEY6",  col, row));
            _buttons.Add(new ButtonViewModel("KEY7",  ++col, row));
            _buttons.Add(new ButtonViewModel("KEY8",  ++col, row));
            _buttons.Add(new ButtonViewModel("KEY9",  ++col, row));
            _buttons.Add(new ButtonViewModel("KEY10", ++col, row));
            col = 0;
            row++;
            _buttons.Add(new ButtonViewModel("KEY11", col, row));
            _buttons.Add(new ButtonViewModel("KEY12", ++col, row));
            _buttons.Add(new ButtonViewModel("KEY13", ++col, row));
            _buttons.Add(new ButtonViewModel("KEY14", ++col, row));
            _buttons.Add(new ButtonViewModel("KEY15", ++col, row));
            col = 0;
            row++;
            _buttons.Add(new ButtonViewModel("KEY16", col, row));
            _buttons.Add(new ButtonViewModel("KEY17", ++col, row));
            _buttons.Add(new ButtonViewModel("KEY18", ++col, row));
            _buttons.Add(new ButtonViewModel("KEY19", ++col, row));
            _buttons.Add(new ButtonViewModel("KEY20", ++col, row));



        }

        #endregion
    }
}