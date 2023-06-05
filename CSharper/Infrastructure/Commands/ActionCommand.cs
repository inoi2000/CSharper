using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CSharper.Infrastructure.Commands
{
    public class ActionCommand : ICommand
    {
        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;
        private readonly string Name;
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public ActionCommand(Action<object> execute, Func<object, bool> canExecute = null,string name=null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
            this.Name = name;
        }

        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }

        public override string ToString()
        { 
            return Name;
        }

    }
}
