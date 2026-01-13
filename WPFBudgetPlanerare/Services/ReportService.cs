using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBudgetPlanerare.DTO;
using WPFBudgetPlanerare.Models;

namespace WPFBudgetPlanerare.Services
{
    public class ReportService
    {
        public decimal GetHourlyRate(User user) // Lägger till för att kunna enklare beräkna timlönen vid sjukdom/VAB?
        {
            if (user.TotalWorkHours <= 0)
            {
                throw new ArgumentException("Anställd måste ha total arbetstid >0 timmar.");
            }
            return user.AnnualIncome / user.TotalWorkHours;

        }
        public decimal GetMonthlyIncome(User user) // Månadsinkomst baseras på timlön och arbetade timmar, dvs mer flexibelt. 
        {
            return user.AnnualIncome / 12;

        }

        public List<TransactionBase> GetTransactionsForMonth(User user, int year, int month)
        {
            return user.Transactions
                .Where(t => t.IsActiveInMonth(year, month))
                .ToList();
        }

        public decimal GetTotalIncomeForMonth(User user, int year, int month)
        {
            var transaction = GetTransactionsForMonth(user, year, month);

            return transaction.OfType<Income>().Sum(t => t.Amount);
        }

        public decimal GetTotalExpensesForMonth(User user, int year, int month)
        {
          var transactions = GetTransactionsForMonth(user, year, month);
            return transactions.OfType<Expense>().Sum(t => t.Amount);

        }



        public MonthlySummary GetMonthlySummary(User user, int year, int month)
        {
            var totalIncome = GetTotalIncomeForMonth(user, year, month);
            var totalExpenses = GetTotalExpensesForMonth(user, year, month);

            return new MonthlySummary
            {
                TotalIncome = totalIncome,
                TotalExpenses = totalExpenses
            };
           


          
           


        }

    }
}
