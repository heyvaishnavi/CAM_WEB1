
using CAM_WEB1.DTOs;
using CAM_WEB1.Models;

namespace CAM_WEB1.Services.Interfaces
{
    public interface ITransactionService
    {
        Task CreateTransaction(TransactionCreateDTO request, string userId);

        Task<List<Transaction>> GetAll();

        Task<Transaction> GetById(string id);
    }
}
