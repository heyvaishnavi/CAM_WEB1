using CAM_WEB1.DTO;
using CAM_WEB1.Models;

namespace CAM_WEB1.Services.Interfaces
{
    public interface IAccountService
    {
        Task<List<AccountResponse>> CreateAccount(AccountCreateRequest request, string userId);
        Task<List<AccountResponse>> UpdateAccount(string accountId, AccountUpdateRequest request, string userId);

        Task CloseAccount(string accountId, string userId);

        Task<AccountResponse?> GetAccountById(string accountId);

        Task<List<AccountResponse>>GetAllAccounts();
    }
}
