

namespace WPFBudgetPlanerare.Models
{
    public enum TransactionFilterType
    {
        All,
        Income,
        Expenses,

    }

    public abstract class Transaction<TEnum> : TransactionBase where TEnum : struct // struct för att begränsa TEnum till värdetyper (enums)

    {

        public TEnum Category { get; set; }


    }
}
