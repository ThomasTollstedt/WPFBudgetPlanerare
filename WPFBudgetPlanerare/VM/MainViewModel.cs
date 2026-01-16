using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Input;
using WPFBudgetPlanerare.Command;
using WPFBudgetPlanerare.Models;
using WPFBudgetPlanerare.Services;

namespace WPFBudgetPlanerare.VM
{
    public class MainViewModel : ViewModelBase
    {
        private readonly Func<User, ICommand, TransactionBase?, AddTransactionViewModel> _addTransactionFactory;
        private readonly Func<User, ICommand, DashboardViewModel> _dashboardFactory;
        private readonly Func<User, ForecastViewModel> _forecastFactory;
        private readonly User _user;

        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set { _currentViewModel = value; RaisePropertyChanged(); }
        }

        public RelayCommand NavigateToDashboardCommand { get; set; }
        public RelayCommand NavigateToForecastCommand { get; set; }
        public RelayCommand NavigateToAddTransactionCommand { get; set; }

        public MainViewModel(
            Func<User, ICommand, TransactionBase?, AddTransactionViewModel> addTransactionFactory,
            Func<User, ICommand, DashboardViewModel> dashboardFactory,
            Func<User, ForecastViewModel> forecastFactory,
            User user)
        {
            _addTransactionFactory = addTransactionFactory;
            _dashboardFactory = dashboardFactory;
            _forecastFactory = forecastFactory;
            _user = user;



            NavigateToAddTransactionCommand = new RelayCommand(o =>
            {
                if (o is TransactionBase transactionToEdit)
                {
                    CurrentViewModel = _addTransactionFactory(_user, NavigateToDashboardCommand, transactionToEdit);
                }
                else
                {
                    CurrentViewModel = _addTransactionFactory(_user, NavigateToDashboardCommand, null);

                }
            });

            NavigateToDashboardCommand = new RelayCommand(o => { CurrentViewModel = _dashboardFactory(_user, NavigateToAddTransactionCommand); });
            NavigateToForecastCommand = new RelayCommand(o => { CurrentViewModel = _forecastFactory(_user/*, _reportService*/); });

            CurrentViewModel = _dashboardFactory(_user, NavigateToAddTransactionCommand); // Sätter start-vyn
        }

    }
}
