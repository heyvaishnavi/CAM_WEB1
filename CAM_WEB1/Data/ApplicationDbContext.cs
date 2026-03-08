using CAM_WEB1.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CAM_WEB1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // -----------------------------
        // USER TABLE
        // -----------------------------
        public DbSet<User> Users { get; set; }

        // -----------------------------
        // ROLE TABLE
        // -----------------------------
        public DbSet<Role> Roles { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Approval> Approvals { get; set; }
        public DbSet<Report> Reports { get; set; }

        public DbSet<ReportAudit> ReportAudits { get; set; }

        // -----------------------------
        // MODEL CONFIGURATION
        // -----------------------------
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //------------------------------------------------
            // USER TABLE CONFIG
            //------------------------------------------------
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("t_User");

                entity.HasKey(e => e.UserID);

                entity.Property(e => e.UserID)
                      .HasMaxLength(10);

                entity.Property(e => e.Name)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(e => e.Email)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(e => e.PasswordHash)
                      .HasMaxLength(255)
                      .IsRequired();

                entity.Property(e => e.RoleID)
                      .HasMaxLength(10)
                      .IsRequired();

                entity.Property(e => e.Branch)
                      .HasMaxLength(100);

                entity.Property(e => e.Status)
                      .HasMaxLength(20);

                entity.Property(e => e.RefreshToken)
                      .HasMaxLength(255);

                entity.Property(e => e.CreatedBy)
                      .HasMaxLength(10);

                entity.Property(e => e.ModifiedBy)
                      .HasMaxLength(10);
            });


            //------------------------------------------------
            // ROLE TABLE CONFIG
            //------------------------------------------------
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("t_Role");

                entity.HasKey(e => e.RoleID);

                entity.Property(e => e.RoleID)
                      .HasMaxLength(10);

                entity.Property(e => e.RoleName)
                      .HasMaxLength(50)
                      .IsRequired();
            });

         

            modelBuilder.Entity<Transaction>()
                .HasKey(t => t.TransactionID);

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasPrecision(18, 2);
            modelBuilder.Entity<Transaction>()
                   .ToTable("t_Transactions")
                   .HasKey(x => x.TransactionID);
            modelBuilder.Entity<Account>()
                   .ToTable("t_Account")
                   .HasKey(x => x.AccountID);
            modelBuilder.Entity<Approval>().ToTable("t_Approvals");
            modelBuilder.Entity<Report>()
                   .ToTable("t_Reports");
                   
            modelBuilder.Entity<ReportAudit>().ToTable("t_ReportAudit");

        }



           
      

        



        public async Task<List<Account>> ExecuteAccountSpAsync(
            string action,
            string? accountId = null,
            string? branch = null,
            string? customerName = null,
            string? customerId = null,
            string? accountType = null,
            decimal? balance = null,
            string? status = null,
            string? userId = null)
        {
            var parameters = new[]
            {
                new SqlParameter("@Action", action),

                new SqlParameter("@AccountID", (object?)accountId ?? DBNull.Value),

                new SqlParameter("@Branch", (object?)branch ?? DBNull.Value),

                new SqlParameter("@CustomerName", (object?)customerName ?? DBNull.Value),

                new SqlParameter("@CustomerID", (object?)customerId ?? DBNull.Value),

                new SqlParameter("@AccountType", (object?)accountType ?? DBNull.Value),

                new SqlParameter("@Balance", (object?)balance ?? DBNull.Value),

                new SqlParameter("@Status", (object?)status ?? DBNull.Value),

                new SqlParameter("@UserID", (object?)userId ?? DBNull.Value)
            };

            return await Accounts
                .FromSqlRaw(
                    "EXEC usp_account_crud @Action,@AccountID,@Branch,@CustomerName,@CustomerID,@AccountType,@Balance,@Status,@UserID",
                    parameters)
                .ToListAsync();
        }
    }

}
