using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WPFBudgetPlanerare.Models;
using WPFBudgetPlanerare.Services;

namespace WPFBudgetPlanerare.VM
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ReportService _reportService;
        private readonly User _user;

        public MainViewModel(ReportService reportService, User user)
        {
            _reportService = reportService;
            _user = user;
        }

        public string UserName
        {
            get { return _user.UserName; }
           
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

        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set { _currentViewModel = value;
                RaisePropertyChanged();
            }
        }

        public MainViewModel()
        {
            
        }


    }
}
