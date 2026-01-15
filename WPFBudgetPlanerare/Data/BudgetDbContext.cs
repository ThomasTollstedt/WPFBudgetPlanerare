using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WPFBudgetPlanerare.Models;

namespace WPFBudgetPlanerare.Data
{
    public class BudgetDbContext : DbContext
    {
        public BudgetDbContext(DbContextOptions<BudgetDbContext> options) : base(options)
        {
        }
        public DbSet<TransactionBase> Transactions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Income> Incomes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Viktigt att behålla denna

            // Konfigurera Amount i TransactionBase
            modelBuilder.Entity<TransactionBase>()
                .Property(t => t.Amount)
                .HasPrecision(18, 2); // Fyll i siffrorna här

            modelBuilder.Entity<User>()
                .Property(u => u.AnnualIncome)
                .HasPrecision(18, 2);
            modelBuilder.Entity<User>()
                .Property(u => u.AnnualIncome)
                .HasPrecision(18, 2);
            modelBuilder.Entity<User>()
                .Property(u => u.TotalWorkHours)
                .HasPrecision(18, 2);
        }

    }
}
