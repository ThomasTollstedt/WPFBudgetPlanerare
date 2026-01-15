using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using WPFBudgetPlanerare.Command;
using WPFBudgetPlanerare.Models;
using WPFBudgetPlanerare.Repositories;
using WPFBudgetPlanerare.Services;

namespace WPFBudgetPlanerare.VM
{
    public class AddTransactionViewModel : ViewModelBase
    {
        private readonly IReportService _reportService;
        private readonly User _user;
        private readonly TransactionBase? _transactionToEdit;

        public AddTransactionViewModel(IReportService reportService,User user, ICommand toDashboardCommand, TransactionBase? transactionToEdit = null)
        {
            _reportService = reportService;
            _user = user;
            _transactionToEdit = transactionToEdit;
            ToDashboardCommand = toDashboardCommand;

            Date = DateTime.Now;
            IsExpense = true;

            //Foreachen behövd?
            foreach (var freq in Enum.GetValues<TransactionFrequency>())
            {
                Frequencies.Add(freq);
            }
            SelectedFrequency = TransactionFrequency.Månatlig; //Default value

            if (transactionToEdit is not null)
            {
                Amount = transactionToEdit.Amount;
                Description = transactionToEdit.Description;
                Date = transactionToEdit.StartDate.ToDateTime(TimeOnly.MinValue);
                if (transactionToEdit is Expense expense)
                {

                    SelectedCategory = expense.Category;
                }
                else if (transactionToEdit is Income income)
                {
                    IsExpense = false;
                    SelectedCategory = income.Category;
                }

                SelectedFrequency = transactionToEdit.Frequency;
            }
            SaveCommand = new RelayCommand(o => SaveTransaction());
        }



        public RelayCommand SaveCommand { get; }
        public ICommand ToDashboardCommand { get; }

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
        private async void SaveTransaction()
        {
            if (Amount <= 0 || string.IsNullOrWhiteSpace(Description))
                return;

            TransactionBase newTransaction;

            if (_transactionToEdit == null) // Lägg till ny transaktion om det inte finns någon att redigera
            {
                if (IsExpense)
                {
                    var expense = new Expense
                    {
                        UserId = _user.Id,
                        Amount = Amount,
                        Description = Description,
                        StartDate = DateOnly.FromDateTime(Date),
                        EndDate = EndDate.HasValue ? DateOnly.FromDateTime(EndDate.Value) : null,
                        Frequency = SelectedFrequency,
                        Category = (ExpenseCategory)SelectedCategory //Konvertering (cast) till rätt Enum-typ för expense
                    };

                    newTransaction = expense;
                }
                else
                {
                    var income = new Income
                    {
                        UserId =  _user.Id,
                        Amount = Amount,
                        Description = Description,
                        StartDate = DateOnly.FromDateTime(Date),
                        Frequency = SelectedFrequency,
                        Category = (IncomeCategory)SelectedCategory
                    };
                    newTransaction = income;
                }
               await _reportService.SaveTransactionAsync(newTransaction);
            }
            else 
            {
                _transactionToEdit.Amount = Amount;
                _transactionToEdit.Description = Description;
                _transactionToEdit.StartDate = DateOnly.FromDateTime(Date);
                _transactionToEdit.EndDate = EndDate.HasValue ? DateOnly.FromDateTime(EndDate.Value) : null;
                _transactionToEdit.Frequency = SelectedFrequency;
                if (_transactionToEdit is Expense expenseToUpdate)
                {
                    expenseToUpdate.Category = (ExpenseCategory)SelectedCategory;
                }
                else if (_transactionToEdit is Income incomeToUpdate)
                {
                    incomeToUpdate.Category = (IncomeCategory)SelectedCategory;
                }
                RaisePropertyChanged();
               await _reportService.UpdateTransactionAsync(_transactionToEdit);
            }
            ClearForm(); // Anropar hjälpmetod för att rensa formuläret efter sparning
            ToDashboardCommand.Execute(null);
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
