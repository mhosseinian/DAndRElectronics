using System;
using System.Windows.Input;

namespace Common.Helpers
{
    public class RelayCommand : ICommand
    {
        #region Fields

        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;
        private EventHandler _internalCanExecuteChanged;

        #endregion // Fields

        #region Constructors

        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion // Constructors

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                _internalCanExecuteChanged += value;

                CommandManager.RequerySuggested += value;
            }

            remove
            {
                // ReSharper disable DelegateSubtraction
                _internalCanExecuteChanged -= value;
                // ReSharper restore DelegateSubtraction

                CommandManager.RequerySuggested -= value;
            }
        }

        public void Execute(object parameter)
        {
            if (_execute != null)
            {
                _execute(parameter);
            }
        }

        public void RaiseCanExecuteChanged()
        {
            if (_canExecute != null)
            {
                EventHandler canExecuteChanged = _internalCanExecuteChanged;

                if (canExecuteChanged != null)
                {
                    canExecuteChanged(this, EventArgs.Empty);
                }
            }
        }

        #endregion
    }
}