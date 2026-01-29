using System.Collections.ObjectModel;
using WPFBudgetPlanerare.Command;
using WPFBudgetPlanerare.Models;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using WPFBudgetPlanerare.Services;

namespace WPFBudgetPlanerare.VM
{

    public class DashboardViewModel : ViewModelBase
    {
        private readonly User _user;
        private readonly IReportService _reportService;

        private decimal _monthlyIncome;
        public decimal MonthlyIncome
        {
            get => _monthlyIncome;
            private set { if (_monthlyIncome != value) { _monthlyIncome = value; RaisePropertyChanged(); } }
        }
        
        private decimal _annualIncome;
        public decimal AnnualIncome
        {
            get => _annualIncome;
            private set { if (_annualIncome != value) { _annualIncome = value; RaisePropertyChanged(); } }
        }

        public DashboardViewModel(/*ITransactionRepository transactionRepo,*/ IReportService reportService, User user, ICommand editCommand)
        {
            _user = user;
            _reportService = reportService;
            EditTransactionCommand = editCommand;

            Transactions = new ObservableCollection<TransactionBase>();
            DeleteCommand = new RelayCommand<TransactionBase>(t => DeleteTransaction(t));
            FilterCommand = new RelayCommand<object>(t => FilterTransaction(t));

            LoadTransactions();
        }

        private async void LoadTransactions()
        {
            var result = await _reportService.GetAllTransactionsAsync(_user.Id);

            Transactions.Clear();
            foreach (var item in result)
            {
                Transactions.Add(item);
            }
            _user.Transactions = result.ToList(); // keep user cache in sync
            RecalculateIncome();
        }

        public ICommand EditTransactionCommand { get; }
        public RelayCommand<TransactionBase> DeleteCommand { get; }
        public RelayCommand<object> FilterCommand { get; }
        public ObservableCollection<TransactionBase> Transactions { get; }
        public ICollectionView TransactionsView => CollectionViewSource.GetDefaultView(Transactions);

        public void ShowOnlyIncome()
        {
            TransactionsView.Filter = t => t is Income;
        }

        public void ShowOnlyExpenses()
        {
            TransactionsView.Filter = t => t is Expense;
        }


        public string UserName => _user.UserName;

        private async Task DeleteTransaction(TransactionBase transaction)
        {
            _user.Transactions.Remove(transaction);
            Transactions.Remove(transaction);
            await _reportService.DeleteTransactionAsync(transaction);
            RecalculateIncome();
        }

        private void RecalculateIncome()
        {
            var now = DateTime.Now;
            MonthlyIncome = _reportService.GetTotalIncomeForMonth(_user, now.Year, now.Month);
            AnnualIncome  = MonthlyIncome * 12;
        }

        public void FilterTransaction(object t)
        {
            if (t is TransactionFilterType filterType)
            {
                switch (filterType)
                {
                    case TransactionFilterType.All:
                        TransactionsView.Filter = null;
                        break;
                    case TransactionFilterType.Income:
                        ShowOnlyIncome();
                        break;
                    case TransactionFilterType.Expenses:
                        ShowOnlyExpenses();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
