using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WPFBudgetPlanerare.Command;
using WPFBudgetPlanerare.Models;
using WPFBudgetPlanerare.Services;

namespace WPFBudgetPlanerare.VM
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ReportService _reportService;
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

        public MainViewModel(ReportService reportService, User user)
        {
            _reportService = reportService;
            _user = user;

            
            
            NavigateToAddTransactionCommand = new RelayCommand(o =>
            {
                if (o is TransactionBase transactionToEdit)
                {
                    CurrentViewModel = new AddTransactionViewModel(_user, NavigateToDashboardCommand, transactionToEdit);
                }
                else
                {
                    CurrentViewModel = new AddTransactionViewModel(_user, NavigateToDashboardCommand);

                }
            });

            NavigateToDashboardCommand = new RelayCommand(o => { CurrentViewModel = new DashboardViewModel(_user, NavigateToAddTransactionCommand); });
            NavigateToForecastCommand = new RelayCommand(o => { CurrentViewModel = new ForecastViewModel(_user, _reportService); });

            CurrentViewModel = new DashboardViewModel(_user, NavigateToAddTransactionCommand); // Sätter start-vyn
        }

    }
}
