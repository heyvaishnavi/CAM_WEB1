using CAM_WEB1.Data;
using CAM_WEB1.DTO;
using CAM_WEB1.DTOs;
using CAM_WEB1.Models;
using CAM_WEB1.Repositories.Interfaces;
using CAM_WEB1.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CAM_WEB1.Services.Implementations
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _repo;
        private readonly ApplicationDbContext _context;

        public TransactionService(ITransactionRepository repo, ApplicationDbContext context)
        {
            _repo = repo;
            _context = context;
        }

        public async Task CreateTransaction(TransactionCreateDTO request, string userId)
        {
            
            if (request.Amount <= 0)
                throw new Exception("Amount must be greater than zero");

            if (string.IsNullOrEmpty(request.FromAccountID))
                throw new Exception("FromAccountID is required");

            var type = request.Type.ToLower();

            if (type != "deposit" && type != "withdraw" && type != "transfer")
                throw new Exception("Invalid transaction type");

            var account = await _context.Accounts
    .FirstOrDefaultAsync(x => x.AccountID == request.FromAccountID);

            
            if (account == null)
                throw new Exception("Account not found");

            if(account.Status == "Closed")
            {
                throw new Exception("Account is Closed");
            }
            decimal balance = account.Balance;

            // Withdraw validation
            if (type == "withdraw")
            {
                if (request.Amount > balance)
                    throw new Exception("Insufficient balance");
            }

            // Transfer validation
            if (type == "transfer")
            {


                if (string.IsNullOrEmpty(request.ToAccountID))
                    throw new Exception("ToAccountID is required for transfer");
                var account2 = await _context.Accounts
    .FirstOrDefaultAsync(x => x.AccountID == request.ToAccountID);

                if (account2.Status == "Closed")
                {
                    throw new Exception("Account is Closed");
                }


                if (request.FromAccountID == request.ToAccountID)
                    throw new Exception("Cannot transfer to same account");

                if (request.Amount > balance)
                    throw new Exception("Insufficient balance for transfer");
            }

            await _repo.CreateTransaction(request, userId);
        }

        public async Task<List<Transaction>> GetAll()
        {
            return await _repo.GetAll();
        }

        public async Task<Transaction> GetById(string id)
        {
            return await _repo.GetById(id);
        }
    }
}
