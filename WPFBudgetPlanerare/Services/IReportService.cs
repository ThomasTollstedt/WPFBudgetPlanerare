using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBudgetPlanerare.DTO;
using WPFBudgetPlanerare.Models;

namespace WPFBudgetPlanerare.Services
{
    public interface IReportService
    {
        decimal GetHourlyRate(User user);
        
        decimal GetMonthlyIncome(User user);

        List<TransactionBase> GetTransactionsForMonth(User user, int year, int month);

        decimal GetTotalIncomeForMonth(User user, int year, int month);

        decimal GetTotalExpensesForMonth(User user, int year, int month);

        MonthlySummary GetMonthlySummary(User user, int year, int month);

    }
}
