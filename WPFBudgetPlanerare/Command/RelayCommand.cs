using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFBudgetPlanerare.Command
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Predicate<object?> _canExecute;

        public RelayCommand(Action<object?> execute, Predicate<object?> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }


        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object? parameter) => _canExecute == null || _canExecute(parameter);


        public void Execute(object? parameter) => _execute(parameter);


    }
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Predicate<T>? _canExecute;
        public RelayCommand(Action<T> execute, Predicate<T>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public bool CanExecute(object? parameter)
        {
            if (_canExecute == null)
                return true;
            if (parameter == null && typeof(T).IsValueType)
                return _canExecute(default!);
            return parameter is T t && _canExecute(t);
        }
        public void Execute(object? parameter)
        {
            if (parameter == null && typeof(T).IsValueType)
            {
                _execute(default!);
                return;
            }
            if (parameter is T t)
            {
                _execute(t);
            }
        }
    }
}
