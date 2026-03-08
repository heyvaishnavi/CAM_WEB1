using CAM_WEB1.Data;
using CAM_WEB1.DTO;
using CAM_WEB1.Models;
using CAM_WEB1.Repositories.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CAM_WEB1.Repositories
{
    public class ApprovalRepository : IApprovalRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly string _conn;

        public ApprovalRepository(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _conn = config.GetConnectionString("DefaultConnection");
        }

        public async Task<List<Approval>> GetAll()
        {
            return await _context.Approvals
                .FromSqlRaw("EXEC usp_Approval @Action='GetAll'")
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Approval> GetById(int id)
        {
            var approvals = await _context.Approvals
                .FromSqlRaw(
                    "EXEC usp_Approval @Action='GetById', @ApprovalId={0}", id)
                .AsNoTracking()
                .ToListAsync();

            return approvals.FirstOrDefault();
        }

        public async Task SubmitDecision(int id, ApprovalDecisionDTO request, string reviewerId)
        {
            try
            {
                var parameters = new[]
                {
                new SqlParameter("@Action","Update"),
                new SqlParameter("@ApprovalId",id),
                new SqlParameter("@ReviewerId",reviewerId),
                new SqlParameter("@Decision",request.Decision),
                new SqlParameter("@Comments",request.Comments ?? "")
            };

                await _context.Database.ExecuteSqlRawAsync(
                    @"EXEC usp_Approval 
                    @Action=@Action,
                    @ApprovalId=@ApprovalId,
                    @ReviewerId=@ReviewerId,
                    @Decision=@Decision,
                    @Comments=@Comments",
                    parameters);

            }
            catch (SqlException ex)
            {
                throw new Exception(ex.Message);
            }

            Audit(reviewerId,
                $"Approval {request.Decision}",
                $"ApprovalID:{id}",
                $"Comments:{request.Comments}");
        }

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
