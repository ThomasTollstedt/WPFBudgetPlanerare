using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBudgetPlanerare.Command;
using WPFBudgetPlanerare.Models;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using WPFBudgetPlanerare.Repositories;
using WPFBudgetPlanerare.Services;

namespace WPFBudgetPlanerare.VM
{

    public class DashboardViewModel : ViewModelBase
    {
        private readonly User _user;
        private readonly IReportService _reportService;

        //private readonly ITransactionRepository _transactionRepo;

        public DashboardViewModel(/*ITransactionRepository transactionRepo,*/ IReportService reportService, User user, ICommand editCommand)
        {
            _user = user;
            _reportService = reportService;
            //_transactionRepo = transactionRepo;
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
            ;
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


        public decimal AnnualIncome
        {
            get
            {
                return _user.AnnualIncome;
            }
        }

        private async Task DeleteTransaction(TransactionBase transaction)
        {
            _user.Transactions.Remove(transaction);
            Transactions.Remove(transaction);
            await _reportService.DeleteTransactionAsync(transaction);
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
