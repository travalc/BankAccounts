using Microsoft.EntityFrameworkCore;

namespace BankAccounts.Models
{
    public class BankAccountsContext : DbContext
    {
        public BankAccountsContext(DbContextOptions<BankAccountsContext> options) : base(options) { } 

        public DbSet<User> users {get; set;}
        public DbSet<Account> accounts {get; set;}
        public DbSet<Transaction> transactions {get; set;}
    }
}