using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
