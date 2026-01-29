using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WPFBudgetPlanerare.Command;
using WPFBudgetPlanerare.Models;
using WPFBudgetPlanerare.Repositories;
using WPFBudgetPlanerare.Services;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WPFBudgetPlanerare.VM
{
    public class ForecastViewModel : ViewModelBase
    {
       
        private readonly User _user;
        private readonly IReportService _reportService;

        public ForecastViewModel(User user, IReportService reportService)
        {
           
            _user = user;
            _reportService = reportService;

            selectedYear = DateTime.Now.Year;
            selectedMonth = DateTime.Now.Month;

            UpdateForecast();
            LoadTransactions();
        }


        private int selectedYear;

        public int SelectedYear
        {
            get { return selectedYear; }
            set
            {
                selectedYear = value;
                RaisePropertyChanged();
                UpdateForecast();
            }
        }

        private int selectedMonth;
        public int SelectedMonth
        {
            get { return selectedMonth; }
            set
            {
                selectedMonth = value;
                RaisePropertyChanged();
                UpdateForecast();
            }
        }

        private decimal predictedIncome;
        public decimal PredictedIncome
        {
            get { return predictedIncome; }
            set { predictedIncome = value; RaisePropertyChanged(); }
        }

        private decimal predictedExpenses;

        public decimal PredictedExpenses
        {
            get { return predictedExpenses; }
            set { predictedExpenses = value; RaisePropertyChanged(); }
        }

        private decimal predictedBalance;

        public decimal PredictedBalance
        {
            get { return predictedBalance; }
            set { predictedBalance = value; RaisePropertyChanged(); }
        }

        //Lista på vilka poster som påverkar denna månads prognos
        private List<TransactionBase> _allTransactions = new List<TransactionBase>();
        public ObservableCollection<TransactionBase> ForecastTransactions { get; set; } = new ObservableCollection<TransactionBase>();


        public string Title => "Månadsprognos";


        private void UpdateForecast()
        {
            PredictedIncome = _reportService.GetTotalIncomeForMonth(_user, SelectedYear, SelectedMonth);
            PredictedExpenses = _reportService.GetTotalExpensesForMonth(_user, SelectedYear, SelectedMonth);

            //räkna ut balans
            PredictedBalance = PredictedIncome - PredictedExpenses;

            UpdateTransactionList();

        }

        private void UpdateTransactionList()
        {
            ForecastTransactions.Clear();

            var filteredTransactions = _allTransactions
                .Where(t => t.IsActiveInMonth(SelectedYear, SelectedMonth))
                .ToList();


            foreach (var t in filteredTransactions)
            {
                ForecastTransactions.Add(t);
            }
        }

        private async void LoadTransactions()
        {
            var result = await _reportService.GetAllTransactionsAsync(_user.Id);
            _allTransactions = result.ToList();
            _user.Transactions = _allTransactions;

            UpdateForecast();

        }

    }
}
