using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;
using WPFBudgetPlanerare.Command;
using WPFBudgetPlanerare.Models;

namespace WPFBudgetPlanerare.VM
{
    public class AddTransactionViewModel : ViewModelBase
    {
        private readonly User _user;

        public AddTransactionViewModel(User user)
        {
            _user = user;

            Date = DateTime.Now;
            IsExpense = true;

            foreach (var freq in Enum.GetValues<TransactionFrequency>())
            {
                Frequencies.Add(freq);
            }
            SelectedFrequency = TransactionFrequency.Månatlig; //Default value

            SaveCommand = new RelayCommand(o => SaveTransaction());
        }

      

        public RelayCommand SaveCommand { get; }


        private bool _isExpense;
        public bool IsExpense
        {
            get { return _isExpense; }
            set
            {
                if (_isExpense != value)
                {
                    _isExpense = value;
                    RaisePropertyChanged();
                    UpdateCategories();
                }
            }
        }



        public ObservableCollection<object> AvailableCategories { get; set; } = new ObservableCollection<object>();

        private object _selectedCategory;

        public object SelectedCategory
        {
            get { return _selectedCategory; }
            set { _selectedCategory = value; RaisePropertyChanged(); }
        }

        private void UpdateCategories()
        {
            AvailableCategories.Clear();
            if (IsExpense)
            {
                foreach (var item in Enum.GetValues(typeof(ExpenseCategory)))
                {
                    AvailableCategories.Add(item);
                }
            }
            else
            {
                foreach (var item in Enum.GetValues(typeof(IncomeCategory)))
                {
                    AvailableCategories.Add(item);
                }
            }

            if (AvailableCategories.Count > 0)
            {
                SelectedCategory = AvailableCategories[0];
            }
        }


        private string _description;

        public string Description
        {
            get { return _description; }
            set { _description = value; RaisePropertyChanged(); }
        }

        private decimal _amount;

        public decimal Amount
        {
            get { return _amount; }
            set { _amount = value; RaisePropertyChanged(); }
        }

        private DateTime _date;

        public DateTime Date
        {
            get { return _date; }
            set { _date = value; RaisePropertyChanged(); }
        }

        private DateTime? _endDate;

        public DateTime? EndDate
        {
            get { return _endDate; }
            set { _endDate = value; RaisePropertyChanged(); }
        }


        //Frekvens (månatlig, årlig).
        public ObservableCollection<TransactionFrequency> Frequencies { get; } = new ObservableCollection<TransactionFrequency>();

        private TransactionFrequency _selectedFrequency;
        public TransactionFrequency SelectedFrequency
        {

            get { return _selectedFrequency; }
            set { _selectedFrequency = value; RaisePropertyChanged(); }

        }
        private void SaveTransaction()
        {
            if (Amount <= 0 || string.IsNullOrWhiteSpace(Description))
                return;

            TransactionBase newTransaction;

                if (IsExpense)
            {
                var expense = new Expense
                {
                    Amount = Amount,
                    Description = Description,
                    StartDate = DateOnly.FromDateTime(Date),
                    EndDate = EndDate.HasValue ? DateOnly.FromDateTime(EndDate.Value) : null,
                    Frequency = SelectedFrequency,
                    Category = (ExpenseCategory)SelectedCategory //Konvertering (cast) till rätt Enum-typ för expense
                };
                //Hantera slutdatum 
                newTransaction = expense;
            }
            else
            {
                var income = new Income
                {
                    Amount = Amount,
                    Description = Description,
                    StartDate = DateOnly.FromDateTime(Date),
                    Frequency = SelectedFrequency,
                    Category = (IncomeCategory)SelectedCategory
                };
                newTransaction = income;
            }
            _user.Transactions.Add(newTransaction);

            ClearForm();

        }

        private void ClearForm()
        {
            Amount = 0;
            Description = string.Empty;

            Date = DateTime.Now;
            EndDate = null;

            SelectedFrequency = TransactionFrequency.Månatlig;


        }
    }
}
