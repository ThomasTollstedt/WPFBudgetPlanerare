using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFBudgetPlanerare.Models
{
  

    public abstract class Transaction<TEnum> : TransactionBase where TEnum : struct
         
    {
        public TEnum Category { get; set; }
        
    }
}
