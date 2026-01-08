using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFBudgetPlanerare.DTO
{
    public class MonthlySummary
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }

        public decimal Balance => TotalIncome - TotalExpenses;



    }
}
