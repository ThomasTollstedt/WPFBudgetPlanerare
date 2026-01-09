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
    }
}
