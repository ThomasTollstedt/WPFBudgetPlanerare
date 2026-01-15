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

namespace WPFBudgetPlanerare.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            

            // Create your dependencies
            //var user = new User(); // Initialize with your user data
           
           
            var reportService = new ReportService();
            //var viewModel = new MainViewModel(reportService, user);
            
            //DataContext = viewModel;
        }
    }
}