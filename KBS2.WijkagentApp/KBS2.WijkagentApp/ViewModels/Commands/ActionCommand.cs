using System;
using System.Windows.Input;

namespace KBS2.WijkagentApp.ViewModels.Commands
{
    public class ActionCommand : ICommand
    {
        /*
         *This class makes it possible to bind actions to a (view)command
         * eg clicking on a button
         */
        private readonly Action<object> action;
        private readonly Predicate<object> canExecute;
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