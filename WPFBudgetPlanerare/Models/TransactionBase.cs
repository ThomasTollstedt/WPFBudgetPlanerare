using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFBudgetPlanerare.Models
{
    public enum TransactionFrequency
    {
        Månatlig,
        Årlig,
        Engångs
    }
    public abstract class TransactionBase
    {
        
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public TransactionFrequency Frequency { get; set; }

        [Range(1, 12)]
        public int? RecurringMonth { get; set; } // Månad för årlig transaktion

        public abstract string CategoryDisplayName { get; }

        public bool IsActiveInMonth(int year, int month)
        {
            DateOnly targetPeriod = new DateOnly(year, month, 1);
            var lastDayOfPeriod = targetPeriod.AddMonths(1).AddDays(-1);

            return Frequency switch
            {
                TransactionFrequency.Engångs => StartDate.Year == year && StartDate.Month == month,
                TransactionFrequency.Månatlig => StartDate <= lastDayOfPeriod && (EndDate == null || EndDate >= targetPeriod),
                TransactionFrequency.Årlig => StartDate.Month == month && StartDate.Year <= year && (EndDate == null || EndDate >= targetPeriod),
                _ => false

            };

        }
    }
}
