using CAM_WEB1.Data;
using CAM_WEB1.Models;
using CAM_WEB1.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CAM_WEB1.Repositories.Implementations
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;

        public AccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Account>> CreateAccount(Account account, string userId)
        {
            var result = await _context.Accounts
                .FromSqlRaw(
                    "EXEC usp_account_crud @Action,@AccountID,@Branch,@CustomerName,@CustomerID,@AccountType,@Balance,@Status,@UserID",
                    new SqlParameter("@Action", "Create"),
                    new SqlParameter("@AccountID", DBNull.Value),
                    new SqlParameter("@Branch", account.Branch),
                    new SqlParameter("@CustomerName", account.CustomerName),
                    new SqlParameter("@CustomerID", DBNull.Value),
                    new SqlParameter("@AccountType", account.AccountType),
                    new SqlParameter("@Balance", account.Balance),
                    new SqlParameter("@Status", "Active"),
                    new SqlParameter("@UserID", userId)
                )
                .ToListAsync();

            await Audit(userId, "CREATE_ACCOUNT", null, $"CustomerID:{account.CustomerID}");

            return result;
        }

        public async Task<List<Account>> UpdateAccount(string accountId, Account account, string userId)
        {
            var result = await _context.Accounts
                .FromSqlRaw(
                    "EXEC usp_account_crud @Action,@AccountID,@Branch,@CustomerName,@CustomerID,@AccountType,@Balance,@Status,@UserID",
                    new SqlParameter("@Action", "Update"),
                    new SqlParameter("@AccountID", accountId),
                    new SqlParameter("@Branch", account.Branch),
                    new SqlParameter("@CustomerName", account.CustomerName),
                    new SqlParameter("@CustomerID", DBNull.Value),
                    new SqlParameter("@AccountType", account.AccountType),
                    new SqlParameter("@Balance", DBNull.Value),
                    new SqlParameter("@Status", account.Status),
                    new SqlParameter("@UserID", userId)
                )
                .ToListAsync();

            await Audit(userId, "UPDATE_ACCOUNT", $"AccountID:{accountId}", $"Status:{account.Status}");

            return result;
        }

        public async Task CloseAccount(string accountId, string userId)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC usp_account_crud @Action,@AccountID,@UserID",
                new SqlParameter("@Action", "Close"),
                new SqlParameter("@AccountID", accountId),
                new SqlParameter("@UserID", userId)
            );

            await Audit(userId, "CLOSE_ACCOUNT", "Active", $"AccountID:{accountId} Closed");
        }

        public async Task<Account?> GetAccountById(string accountId)
        {
            var result = await _context.Accounts
                .FromSqlRaw(
                    "EXEC usp_account_crud @Action,@AccountID",
                    new SqlParameter("@Action", "GetById"),
                    new SqlParameter("@AccountID", accountId)
                )
                .ToListAsync();

            return result.FirstOrDefault();
        }

        public async Task<List<Account>> GetAllAccounts()
        {
            return await _context.Accounts
                .FromSqlRaw(
                    "EXEC usp_account_crud @Action",
                    new SqlParameter("@Action", "GetAll")
                )
                .ToListAsync();
        }

        private async Task Audit(string userId, string action, string? oldVal, string? newVal)
        {
            await _context.Database.ExecuteSqlRawAsync(
                "EXEC usp_user_audit @UserID,@Action,@OldValue,@NewValue",
                new SqlParameter("@UserID", userId),
                new SqlParameter("@Action", action),
                new SqlParameter("@OldValue", (object?)oldVal ?? DBNull.Value),
                new SqlParameter("@NewValue", (object?)newVal ?? DBNull.Value)
            );
        }
    }
}
