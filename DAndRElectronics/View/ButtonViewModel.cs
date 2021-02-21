using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using DAndRElectronics.Helpers;

namespace DAndRElectronics.View
{
    public class ButtonViewModel:ViewModel
    {
        public ICommand OnEdit { get; }

        public string Name { get; set; }

        public int Column { get; set; }
        public int Row { get; set; }

        #region Contructors

        public ButtonViewModel(string name, int col, int row)
        {
            Name = name;
            Column = col;
            Row = row;
            OnEdit = new RelayCommand(OnButtonEdit, o => true);
           
        }

        private void OnButtonEdit(object obj)
        {
            
        }

        #endregion
    }
}