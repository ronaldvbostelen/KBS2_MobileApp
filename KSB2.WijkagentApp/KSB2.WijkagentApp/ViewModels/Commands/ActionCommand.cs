using System;
using System.Windows.Input;

namespace KSB2.WijkagentApp.ViewModels.Commands
{
    public class ActionCommand : ICommand
    {
        readonly Action<object> action;
        readonly Predicate<object> canExecute;
        private EventHandler eventHandler;

        public ActionCommand(Action<object> action) : this(action, null) { }

        public ActionCommand(Action<object> action, Predicate<object> canExecute)
        {
            this.action = action;
            this.canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                eventHandler += value;
            }
            remove
            {
                eventHandler -= value;
            }
        }

        public void RaiseCanExecuteChanged()
        {
            eventHandler?.Invoke(this, new EventArgs());
        }

        public bool CanExecute(object parameter)
        {
            return canExecute?.Invoke(parameter) ?? true;
        }

        public void Execute(object parameter)
        {
            if (CanExecute(parameter)) action(parameter);
        }
    }
}