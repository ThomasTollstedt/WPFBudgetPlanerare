using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public decimal GetTotalIncomeForMonth(User user, int month, int year)
        {
            var income = user.Transactions
                .OfType<Income>()
                .Where(t => t.StartDate.Year == year && t.StartDate.Month == month)
                .Sum(t => t.Amount);

            return income;   
        }

    }
}
