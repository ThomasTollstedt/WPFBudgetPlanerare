using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFBudgetPlanerare.Data;
using WPFBudgetPlanerare.Models;

namespace WPFBudgetPlanerare.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly BudgetDbContext _context;

        public TransactionRepository(BudgetDbContext context)
        {
            _context = context;
        }

        public async Task AddTransactionAsync(TransactionBase transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TransactionBase>> GetAllTransactionsAsync(int userId)
        {
            var allTransanctions = await _context.Transactions
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.StartDate)
                .ToListAsync();

            return allTransanctions;
        }

        public async Task RemoveTransactionAsync(TransactionBase transaction)
        {
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTransactionASync(TransactionBase transaction)
        {
            var transactionToEdit = await  _context.Transactions.FirstOrDefaultAsync(t => t.Id == transaction.Id);
            if (transactionToEdit != null) 
            {
                // Kopiera värdena från inparameter till Db - använde .Update initialt men då trackas Id't multipelt. 
                _context.Entry(transactionToEdit).CurrentValues.SetValues(transaction);
                
                await _context.SaveChangesAsync();
            }
        }
    }
}
