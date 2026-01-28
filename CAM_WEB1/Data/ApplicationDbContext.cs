using CAM_WEB1.Models;
using Microsoft.EntityFrameworkCore;

namespace CAM_WEB1.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options) { }

		public DbSet<Account> Accounts { get; set; }
		public DbSet<Transaction> Transactions { get; set; } // Add this line
	}
}