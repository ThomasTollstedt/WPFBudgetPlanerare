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

                    // 2. Registrera dina Repositories (Koppla Interface till Klass)
                    // Transient = Ny instans varje gång någon ber om den
                    services.AddTransient<ITransactionRepository, TransactionRepository>();

                    // 3. Registrera dina Services
                    services.AddTransient<IReportService, ReportService>();
                    services.AddSingleton<Func<User, ICommand, TransactionBase?, AddTransactionViewModel>>(provider =>
                            (user, navCommand, transaction) =>
                        {
                         // 1. Här hämtar fabriken Repot från containern (som MainViewModel inte visste om)
                         var repo = provider.GetRequiredService<ITransactionRepository>();

                            // 2. Här bygger fabriken bilen. 
                            // Hur ser koden ut för att returnera din ViewModel här?
                            return new AddTransactionViewModel(repo, user, navCommand, transaction);
                            
                           
                        });

                    services.AddSingleton<Func<User, ICommand, DashboardViewModel>>(provider =>
                            (user, navCommand ) =>
                            {
                                // 1. Här hämtar fabriken Repot från containern (som MainViewModel inte visste om)
                                var repo = provider.GetRequiredService<ITransactionRepository>();

                                // 2. Här bygger fabriken bilen. 
                                // Hur ser koden ut för att returnera din ViewModel här?
                                return new DashboardViewModel(repo, user, navCommand);


                            });

                    services.AddSingleton<Func<User, IReportService, ForecastViewModel>>(provider =>
                           (user, reportService) =>
                           {
                               // 1. Här hämtar fabriken Repot från containern (som MainViewModel inte visste om)
                               var repo = provider.GetRequiredService<ITransactionRepository>();

                               // 2. Här bygger fabriken bilen. 
                               // Hur ser koden ut för att returnera din ViewModel här?
                               return new ForecastViewModel(repo, user, reportService);


                           });

                    var dummyUser = new User
                    {
                        Id = 1,
                        UserName = "Admin",
                        AnnualIncome = 550000m,
                        TotalWorkHours = 1980m

                        // Eller vilka properties din User-klass nu har
                    };

                    // 4. Registrera ViewModels & Views
                    services.AddSingleton(dummyUser);
                    services.AddSingleton<MainViewModel>();
                    services.AddSingleton<MainWindow>();
                    //services.AddSingleton<DashboardViewModel>();
                    services.AddSingleton<DashboardView>();
                    //services.AddSingleton<ForecastViewModel>();
                    services.AddSingleton<ForecastView>();
                    //services.AddSingleton<AddTransactionViewModel>();
                    services.AddSingleton<AddTransactionView>();
                })
                .Build();
        }



        protected override async void OnStartup(StartupEventArgs e)
        {

            await AppHost!.StartAsync();

            // Hämta MainWindow från DI-containern så att alla beroenden följer med!
            var startupForm = AppHost.Services.GetRequiredService<MainWindow>();

            // Hämta ViewModel och sätt som DataContext
            startupForm.DataContext = AppHost.Services.GetRequiredService<MainViewModel>();

            startupForm.Show();

            base.OnStartup(e);
        }
    }

}
