using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBudgetPlanerare.VM;

namespace WPFBudgetPlanerare.Models
{
    public enum TransactionFrequency
    {
        Månatlig,
        Årlig,
        Engångs
    }
    public abstract class TransactionBase : ViewModelBase // Ärver från ViewModelBase för att möjliggöra PropertyChanged-händelser vid EDIT
    {

        public int Id { get; set; }

       
        private decimal _amount;
        public decimal Amount
        {
            get { return _amount; }
            set
            {
                if (_amount != value)
                {
                    _amount = value; RaisePropertyChanged();
                }

            }
        }

        private string? _description;
        public string? Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value; RaisePropertyChanged();
                }
            }
        }

        private DateOnly _startDate;
        public DateOnly StartDate
        {
            get { return _startDate; }

            set
            {
                if (_startDate != value)
                {
                    _startDate = value; RaisePropertyChanged();
                }

            }

        }

        private DateOnly? _endDate;
        public DateOnly? EndDate
        {

            get { return _endDate; }

            set
            {
                if (_endDate != value)
                {
                    _endDate = value; RaisePropertyChanged();
                }
            }

        }

        private TransactionFrequency _frequency;
        public TransactionFrequency Frequency
        {

            get { return _frequency; }
            set
            {
                _frequency = value; RaisePropertyChanged();
            }

        }

        [Range(1, 12)]
        public int? RecurringMonth { get; set; } // Månad för årlig transaktion

        public abstract string CategoryDisplayName { get; }

        public bool IsActiveInMonth(int year, int month)
        {
            DateOnly targetPeriod = new DateOnly(year, month, 1);
            var lastDayOfPeriod = targetPeriod.AddMonths(1).AddDays(-1);

            return Frequency switch
            {
                TransactionFrequency.Engångs => StartDate.Year == year && StartDate.Month == month,
                TransactionFrequency.Månatlig => StartDate <= lastDayOfPeriod && (EndDate == null || EndDate >= targetPeriod),
                TransactionFrequency.Årlig => StartDate.Month == month && StartDate.Year <= year && (EndDate == null || EndDate >= targetPeriod),
                _ => false

            };

        }
    }
}
