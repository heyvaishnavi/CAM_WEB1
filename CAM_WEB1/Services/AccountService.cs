using CAM_WEB1.DTO;
using CAM_WEB1.Models;
using CAM_WEB1.Repositories.Interfaces;
using CAM_WEB1.Services.Interfaces;

namespace CAM_WEB1.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repo;

        public AccountService(IAccountRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<AccountResponse>> CreateAccount(AccountCreateRequest request, string userId)
        {
            var account = new Account
            {
                CustomerName = request.CustomerName,
                AccountType = request.AccountType,
                Balance = request.Balance,
                Branch = request.Branch
            };

            var result = await _repo.CreateAccount(account, userId);

            return result.Select(Map).ToList();
        }

        public async Task<List<AccountResponse>> UpdateAccount(string id, AccountUpdateRequest request, string userId)
        {
            var account = new Account
            {
                CustomerName = request.CustomerName,
                AccountType = request.AccountType,
                Branch = request.Branch,
                Status = request.Status
            };

            var result = await _repo.UpdateAccount(id, account, userId);

            return result.Select(Map).ToList();
        }

        public async Task CloseAccount(string id, string userId)
        {
            await _repo.CloseAccount(id, userId);
        }

        public async Task<AccountResponse?> GetAccountById(string id)
        {
            var result = await _repo.GetAccountById(id);

            if (result == null) return null;

            return Map(result);
        }

        public async Task<List<AccountResponse>> GetAllAccounts()
        {
            var result = await _repo.GetAllAccounts();

            return result.Select(Map).ToList();
        }

        private AccountResponse Map(Account a)
        {
            return new AccountResponse
            {
                AccountID = a.AccountID,
                CustomerID = a.CustomerID,
                CustomerName = a.CustomerName,
                AccountType = a.AccountType,
                Balance = a.Balance,
                Status = a.Status,
                Branch = a.Branch,
                CreatedBy = a.CreatedBy,
                CreatedDate = a.CreatedDate,
                ModifiedBy = a.ModifiedBy,
                ModifiedDate = a.ModifiedDate
            };
        }
    }
}
