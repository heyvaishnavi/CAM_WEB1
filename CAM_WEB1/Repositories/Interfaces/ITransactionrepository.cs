using CAM_WEB1.DTOs;
using CAM_WEB1.Models;

namespace CAM_WEB1.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        // Create transaction (Deposit / Withdraw / Transfer)
        Task CreateTransaction(  TransactionCreateDTO
 request, string userId);

        // Get all transactions
        Task<List<Transaction>> GetAll();

        // Get transaction by ID
        Task<Transaction> GetById(string id);
    }
}
