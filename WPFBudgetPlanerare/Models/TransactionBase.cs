


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
        
        public abstract string CategoryDisplayName { get; }

        public int UserId { get; set; }
        public virtual User User { get; set; }



        public bool IsActiveInMonth(int year, int month)
        {
            DateOnly targetPeriod = new DateOnly(year, month, 1);
            var lastDayOfPeriod = targetPeriod.AddMonths(1).AddDays(-1);

            return Frequency switch
            {
                TransactionFrequency.Engångs => StartDate.Year == year && StartDate.Month == month,
                // Månatlig -> kollar om transaktioner överlappar, dvs start innan månadens sista dag OCH slutdatum är null/efter månadens första dag
                TransactionFrequency.Månatlig => StartDate <= lastDayOfPeriod && (EndDate == null || EndDate >= targetPeriod),
                // Årlig -> Check om Month matchar OCH År är före/samma, EndDate null/aktiv vid månadens start. 
                TransactionFrequency.Årlig => StartDate.Month == month && StartDate.Year <= year && (EndDate == null || EndDate >= targetPeriod),
                _ => false

            };

        }
    }
}
