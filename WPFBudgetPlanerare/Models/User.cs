
namespace WPFBudgetPlanerare.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } = "";

        public decimal AnnualIncome { get; set; }
        public decimal TotalWorkHours { get; set; }
        public virtual List<TransactionBase> Transactions { get; set; } = new List<TransactionBase>(); // Virtual för lazy loading och slippa .Include vid hämtning från DB nu vid mindre projekt.


    }
}
