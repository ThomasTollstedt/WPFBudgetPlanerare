using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBudgetPlanerare.Command;
using WPFBudgetPlanerare.Models;

namespace WPFBudgetPlanerare.VM
{
    public class DashboardViewModel : ViewModelBase
    {
        private readonly User _user;

        public DashboardViewModel(User user)
        {
            _user = user;

            var transactions = _user.Transactions
            .OrderByDescending(t => t.StartDate)
            .ToList();

            Transactions = new ObservableCollection<TransactionBase>(transactions);

            DeleteCommand = new RelayCommand<TransactionBase>(t => DeleteTransaction(t));
        }



        public RelayCommand<TransactionBase> DeleteCommand { get; }
        public ObservableCollection<TransactionBase> Transactions { get; }

        public string UserName => _user.UserName;


        public decimal AnnualIncome
        {
            get
            {
                return _user.AnnualIncome;
            }
        }

        
        

        private void DeleteTransaction(TransactionBase transaction)
        {
            _user.Transactions.Remove(transaction);
            Transactions.Remove(transaction);
        }

    }
}
