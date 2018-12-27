using System;
using System.Windows.Input;

namespace KSB2.WijkagentApp.ViewModels.Commands
{
    public class ActionCommand : ICommand
    {
        readonly Action<object> _action;
        readonly Predicate<object> _canExecute;
        private EventHandler _eventHandler;

        public ActionCommand(Action<object> action) : this(action, null) { }

        public ActionCommand(Action<object> action, Predicate<object> canExecute)
        {
            this._action = action;
            this._canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                _eventHandler += value;
            }
            remove
            {
                _eventHandler -= value;
            }
        }

        public void RaiseCanExecuteChanged()
        {
            _eventHandler?.Invoke(this, new EventArgs());
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter)) _action(parameter);
        }
    }
}