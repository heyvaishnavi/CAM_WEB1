using CAM_WEB1.Data;
using CAM_WEB1.DTO;
using CAM_WEB1.DTOs;
using CAM_WEB1.Models;
using CAM_WEB1.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CAM_WEB1.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string _conn;

        public TransactionRepository(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _conn = config.GetConnectionString("DefaultConnection");
        }

        // CREATE TRANSACTION
        public async Task CreateTransaction(TransactionCreateDTO request, string userId)
        {
            try
            {
                var parameters = new[]
                {
                    new SqlParameter("@Action","CREATE"),
                    new SqlParameter("@FromAccountID",request.FromAccountID),
                    new SqlParameter("@ToAccountID",(object?)request.ToAccountID ?? DBNull.Value),
                    new SqlParameter("@Type",request.Type),
                    new SqlParameter("@Amount",request.Amount),
                    new SqlParameter("@CreatedBy",userId)
                };

                await _context.Database.ExecuteSqlRawAsync(
                @"EXEC usp_Transaction 
                    @Action=@Action,
                    @FromAccountID=@FromAccountID,
                    @ToAccountID=@ToAccountID,
                    @Type=@Type,
                    @Amount=@Amount,
                    @CreatedBy=@CreatedBy",
                parameters);

                Audit(userId,
                    "CREATE_TRANSACTION",
                    null,
                    $"Type:{request.Type}, Amount:{request.Amount}");
            }
            catch (SqlException ex)
            {
                throw new Exception("Database error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Transaction failed: " + ex.Message);
            }
        }

        // GET ALL TRANSACTIONS
        public async Task<List<Transaction>> GetAll()
        {
            try
            {
                return await _context.Transactions
                    .FromSqlRaw("EXEC usp_Transaction @Action='GETALL'")
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving transactions: " + ex.Message);
            }
        }

        // GET TRANSACTION BY ID
        public async Task<Transaction> GetById(string id)
        {
            try
            {
                var list = await _context.Transactions
                    .FromSqlRaw(
                    "EXEC usp_Transaction @Action='GETBYID', @TransactionID={0}", id)
                    .AsNoTracking()
                    .ToListAsync();

                return list.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception("Error retrieving transaction: " + ex.Message);
            }
        }

        // AUDIT LOG
        private void Audit(string userId, string action, string oldVal, string newVal)
        {
            using var con = new SqlConnection(_conn);
            using var cmd = new SqlCommand("usp_user_audit", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserID", userId);
            cmd.Parameters.AddWithValue("@Action", action);
            cmd.Parameters.AddWithValue("@OldValue", (object?)oldVal ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@NewValue", (object?)newVal ?? DBNull.Value);

            con.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
