using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBudgetPlanerare.Models;
using WPFBudgetPlanerare.Services;
using WPFBudgetPlanerare.Command;
using System.Collections.ObjectModel;
using System.Transactions;

namespace WPFBudgetPlanerare.VM
{
    public class ForecastViewModel : ViewModelBase
    {
        private readonly User _user;
        private readonly ReportService _reportService;

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

        private decimal predictedExpenses   ;

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
        public ObservableCollection<TransactionBase> ForecastTransactions { get; set; } = new ObservableCollection<TransactionBase>();


        public string Title => "Månadsprognos";

        public ForecastViewModel(User user, ReportService reportService)
        {
            _user = user;
            _reportService = reportService;

            selectedYear = DateTime.Now.Year;
            selectedMonth = DateTime.Now.Month;

            UpdateForecast();
        }

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

            var transactions = _reportService.GetTransactionsForMonth(_user, SelectedYear, SelectedMonth);

            foreach (var t in transactions)
            {
                ForecastTransactions.Add(t);
            }

        }

       
    }
}
