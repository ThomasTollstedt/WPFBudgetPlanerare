

namespace WPFBudgetPlanerare.Models
{
   

    public enum ExpenseCategory
    {
        HusDrift,
        Mat,
        Transport,
        SaaS,
        Barn,
        Streamingtjänster,
        Fritid
    }
    public class Expense : Transaction<ExpenseCategory>
    {
        public override string CategoryDisplayName => Category.ToString();

    }

}
