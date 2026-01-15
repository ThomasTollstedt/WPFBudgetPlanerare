using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using WPFBudgetPlanerare.Data;
using WPFBudgetPlanerare.Models;
using WPFBudgetPlanerare.Repositories;
using WPFBudgetPlanerare.Services;
using WPFBudgetPlanerare.Views;
using WPFBudgetPlanerare.VM;

namespace WPFBudgetPlanerare
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public static IHost? AppHost { get; private set; }
        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {

                    services.AddDbContext<BudgetDbContext>(options =>
                    {
                        options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BudgetPlanerare;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30");
                    });

                    // Repolager för transaktioner
                    services.AddTransient<ITransactionRepository, TransactionRepository>();

                    // Servicelager för rapportering
                    services.AddTransient<IReportService, ReportService>();

                    //User 
                    services.AddSingleton(new User
                        {
                        Id = 1,
                        UserName = "Admin",
                        AnnualIncome = 550000m,
                        TotalWorkHours = 1920 
                    });

                    //Factory for AddTransactionVM
                    services.AddSingleton<Func<User, ICommand, TransactionBase?, AddTransactionViewModel>>(provider =>
                            (user, navCommand, transaction) =>
                        {
                            var reportService = provider.GetRequiredService<IReportService>();
                            return new AddTransactionViewModel(reportService, user, navCommand, transaction);
                        });

                    //Factory DashboardVM
                    services.AddSingleton<Func<User, ICommand, DashboardViewModel>>(provider =>
                            (user, navCommand ) =>
                            {
                                 var reportService = provider.GetRequiredService<IReportService>();
                                return new DashboardViewModel(reportService, user, navCommand);
                            });


                    //Factory for ForecastVM
                    services.AddSingleton<Func<User, IReportService, ForecastViewModel>>(provider =>
                           (user, reportService) =>
                           {
                               return new ForecastViewModel(user, reportService);
                           });

                    services.AddSingleton<MainViewModel>();
                    
                    
                    services.AddSingleton<MainWindow>();     
                    services.AddSingleton<DashboardView>();             
                    services.AddSingleton<ForecastView>();                 
                    services.AddSingleton<AddTransactionView>();
                })
                .Build();
        }



        protected override async void OnStartup(StartupEventArgs e)
        {

            await AppHost!.StartAsync();

          
            var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
            startupForm.DataContext = AppHost.Services.GetRequiredService<MainViewModel>();

            startupForm.Show();

            base.OnStartup(e);
        }
    }

}
