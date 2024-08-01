using BankApp.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace BankApp.Server.Database
{
	public class BankDbContext : DbContext
	{
        public BankDbContext(DbContextOptions<BankDbContext> options) : base(options)
        {
            
        }

        public DbSet<Account> accounts { get; set; }
        public DbSet<Transaction> transactions { get; set; }
    }
}
