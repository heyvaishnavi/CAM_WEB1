using CAM_WEB1.Models;

namespace CAM_WEB1.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task<List<Account>> CreateAccount(Account account, string userId);

        Task<List<Account>> UpdateAccount(string accountId, Account account, string userId);

        Task CloseAccount(string accountId, string userId);

        Task<Account> GetAccountById(string accountId);

        Task<List<Account>> GetAllAccounts();
    }
}

