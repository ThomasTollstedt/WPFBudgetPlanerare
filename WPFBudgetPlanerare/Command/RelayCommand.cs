using System.Windows.Input;

namespace WPFBudgetPlanerare.Command
{

    // Icke generisk RelayCommand-implementation för Navigationen, dvs behöver inte någon data från UI't.
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

        // Metod som avgör om kommandot kan köras.
        public bool CanExecute(object? parameter) => _canExecute == null || _canExecute(parameter);
        // Metod som körs när kommandot exekveras.
        public void Execute(object? parameter) => _execute(parameter);


    }

    // Generisk när knappen skickar med ett värde från UI't.
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
            // Varje gång WPF misstänker UI ändrats, kollar den CanExecute igen för att se om knappen ska vara aktiverad eller inaktiverad.
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public bool CanExecute(object? parameter)
        {
            if (_canExecute == null)
                return true;
            if (parameter == null && typeof(T).IsValueType) // int, bool, double, datetime
                return _canExecute(default!); // ger default värde för värdetyper, ex int = 0, bool = false. 
            return parameter is T t && _canExecute(t); // Pattern matching : är param av typen T?, Om stämmer -> skapa ny variabel t. 
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
