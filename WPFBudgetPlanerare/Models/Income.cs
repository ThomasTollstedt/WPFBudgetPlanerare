using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
