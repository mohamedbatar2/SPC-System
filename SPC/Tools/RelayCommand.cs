using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SPC.Tools
{
    public class RelayCommand : ICommand
    {
        private ICommand resetCommand;
        private Func<object, bool> value;

        public Action<object> _execute { get; set; }
        public Predicate<object> _canExecut { get; set; }
        public RelayCommand(Action<object> execute, Predicate<object> canExecut)
        {
            _execute = execute;
            _canExecut = canExecut;
        }

        public RelayCommand(ICommand resetCommand, Func<object, bool> value)
        {
            this.resetCommand = resetCommand;
            this.value = value;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _canExecut(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}
