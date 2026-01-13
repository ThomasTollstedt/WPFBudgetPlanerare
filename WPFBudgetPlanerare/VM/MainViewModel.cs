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

            NavigateToDashboardCommand = new RelayCommand(o => { CurrentViewModel = new DashboardViewModel(_user); });
            NavigateToForecastCommand = new RelayCommand(o => { CurrentViewModel = new ForecastViewModel(_user, _reportService); });
            NavigateToAddTransactionCommand = new RelayCommand(o => { CurrentViewModel = new AddTransactionViewModel(_user); });

            CurrentViewModel = new DashboardViewModel(_user); // Sätter start-vyn
        }

    }
}
