using WPFBudgetPlanerare.Models;

namespace WPFBudgetPlanerare.Repositories
{
    public interface ITransactionRepository
    {
        Task AddTransactionAsync(TransactionBase transaction);
        Task RemoveTransactionAsync(TransactionBase transaction);
        Task UpdateTransactionASync(TransactionBase transaction);
        Task<List<TransactionBase>> GetAllTransactionsAsync(int userId);

    }
}
