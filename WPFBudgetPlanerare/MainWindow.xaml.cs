using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFBudgetPlanerare.Models;
using WPFBudgetPlanerare.Services;
using WPFBudgetPlanerare.VM;

namespace WPFBudgetPlanerare
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var user = new User 
            { 
            Id = 1, 
            UserName = "Thomas",
            AnnualIncome = 60000m,
            TotalWorkHours = 1600m

            };

            var income = new Income
            {
                Amount = 5000m,
                StartDate = new DateOnly(2024, 1, 1),
                Frequency = TransactionFrequency.Månatlig,
                Description = "Systemutveckling",
                Category = IncomeCategory.Lön
            };
            var expense = new Expense
            {
                Amount = 1500m,
                StartDate = new DateOnly(2024, 1, 5),
                Frequency = TransactionFrequency.Månatlig,
                Description = "Fyfan",
                Category = ExpenseCategory.HusDrift
            };
            var expense2 = new Expense
            {
                Amount = 1500m,
                StartDate = new DateOnly(2024, 1, 5),
                Frequency = TransactionFrequency.Månatlig,
                Description = "Fsda",
                Category = ExpenseCategory.Barn
            };
            var expense3 = new Expense
            {
                Amount = 1500m,
                StartDate = new DateOnly(2024, 1, 5),
                EndDate = new DateOnly(2024,12,5),
                Frequency = TransactionFrequency.Månatlig,
                Description = "ihih",
                Category = ExpenseCategory.Mat
            };


            // Create your dependencies
            //var user = new User(); // Initialize with your user data
           
            
            user.Transactions.Add(income);
            user.Transactions.Add(expense);
            user.Transactions.Add(expense2);
            user.Transactions.Add(expense3);
            var reportService = new ReportService();
            var viewModel = new MainViewModel(reportService, user);
            
            DataContext = viewModel;
        }
    }
}