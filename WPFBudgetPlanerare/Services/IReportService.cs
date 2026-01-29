using WPFBudgetPlanerare.DTO;
using WPFBudgetPlanerare.Models;

namespace WPFBudgetPlanerare.Services
{
    public interface IReportService
    {
        decimal GetHourlyRate(User user);
        decimal GetMonthlyIncome(User user);
        MonthlySummary GetMonthlySummary(User user, int year, int month);
        decimal GetTotalExpensesForMonth(User user, int year, int month);
        decimal GetTotalIncomeForMonth(User user, int year, int month);
        List<TransactionBase> GetTransactionsForMonth(User user, int year, int month);
        
        
        Task<List<TransactionBase>> GetAllTransactionsAsync(int userId);
        Task SaveTransactionAsync(TransactionBase transaction);
        Task UpdateTransactionAsync(TransactionBase transaction);
        Task DeleteTransactionAsync(TransactionBase transaction);
    }
}
