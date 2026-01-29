

namespace WPFBudgetPlanerare.Models
{
    public enum IncomeCategory
    {
        Lön,
        Bidrag,
        Hobbyverksamhet,
    }
    public class Income : Transaction<IncomeCategory>
    {
        public override string CategoryDisplayName => Category.ToString();
    }
}
