using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TaskMasterPRO.Core
{
    public class RelayCommand(
        Action<object> execute,
        Func<object, bool>? canExecute = null
        ) : ICommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value; 
        }

        public bool CanExecute(object? parameter) => canExecute?.Invoke(parameter!) ?? true;
        public void Execute(object? parameter) => execute(parameter!);
    }
}
