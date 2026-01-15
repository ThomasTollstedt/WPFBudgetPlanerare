using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
