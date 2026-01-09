using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBudgetPlanerare.Models;

namespace WPFBudgetPlanerare.VM
{
    public class DashboardViewModel : ViewModelBase
    {
        private readonly User _user;

        public DashboardViewModel(User user)
        {
            _user = user;
        }

        public string UserName => _user.UserName;
      

        public decimal AnnualIncome
        {
            get
            {

                return _user.AnnualIncome;
            }
        }

        public ObservableCollection<TransactionBase> Transactions
        {
            get
            {
                var transactions = _user.Transactions
                    /*.*//*OfType<TransactionBase>()*/
                    .OrderBy(t => t.StartDate)
                    .ToList();
                return new ObservableCollection<TransactionBase>(transactions);
            }
        }

    }
}
