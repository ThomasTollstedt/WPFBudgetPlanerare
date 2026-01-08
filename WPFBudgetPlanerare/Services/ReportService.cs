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
                throw new ArgumentException("Anställd måste ha total arbetstid  över 0 timmar.");
            }
            return user.AnnualIncome / user.TotalWorkHours;

        }
        public decimal GetMonthlyIncome(User user) // Månadsinkomst baseras på timlön och arbetade timmar, dvs mer flexibelt. 
        {
            return user.AnnualIncome / 12;

        }

        public decimal GetTotalIncomeForMonth(User user, int month, int year)
        {
            var monthlyIncome = user.Transactions
                .OfType<Income>()
                .Where(t => t.StartDate.Year == year && t.StartDate.Month == month)
                .Sum(t => t.Amount);

            return monthlyIncome;
        }

        public decimal GetTotalExpensesForMonth(User user, int month, int year)
        {
            DateOnly targetPeriod = new DateOnly(year, month, 1);
            var lastDayOfPeriod = targetPeriod.AddMonths(1).AddDays(-1);

            var monthlyExpenses = user.Transactions
                .OfType<Expense>()
                .Where(t => //Villkoren för att inkludera transaktioner baserat på deras frekvens
                (t.Frequency == TransactionFrequency.Engångs && t.StartDate.Year == year && t.StartDate.Month == month)
                ||
                (t.Frequency == TransactionFrequency.Månatlig && t.StartDate <= lastDayOfPeriod && (t.EndDate == null || t.EndDate >= targetPeriod))
                ||
                (t.Frequency == TransactionFrequency.Årlig && t.StartDate.Month == month && t.StartDate.Year <= year && (t.EndDate == null || t.EndDate >= targetPeriod)))
                .Sum(t => t.Amount);

            return monthlyExpenses;

        }

        public MonthlySummary GetMonthlySummary(User user, int month, int year)
        {
            var totalIncome = GetTotalIncomeForMonth(user, month, year);
            var totalExpenses = GetTotalExpensesForMonth(user, month, year);

            return new MonthlySummary
            {
                TotalIncome = totalIncome,
                TotalExpenses = totalExpenses
            };
           


        }

    }
}
