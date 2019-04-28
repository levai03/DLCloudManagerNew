using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DLCloudManager
{
    class DelegateCommand : ICommand
    {
        private readonly Action<object> _executeAction;
        public DelegateCommand(Action<object> executeAction)
        {
            _executeAction = executeAction;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => _executeAction(parameter);
    }
}
