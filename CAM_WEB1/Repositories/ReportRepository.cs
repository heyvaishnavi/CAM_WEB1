using CAM_WEB1.Data;
using CAM_WEB1.Models;
using CAM_WEB1.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace CAM_WEB1.Repositories.Implementations
{
    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDbContext _context;

        public ReportRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Report> GenerateSnapshot(DateTime from, DateTime to, string branch, string userId)
        {
            var pMode = new SqlParameter("@Mode", "GENERATE");
            var pFrom = new SqlParameter("@FromDate", from);
            var pTo = new SqlParameter("@ToDate", to);
            var pBranch = new SqlParameter("@BranchName", branch);

            await _context.Database.ExecuteSqlRawAsync(
                "EXEC usp_Reports_Master @Mode,@BranchName,@FromDate,@ToDate",
                pMode, pBranch, pFrom, pTo);

            var latest = await _context.Reports
                .OrderByDescending(x => x.ReportID)
                .FirstOrDefaultAsync();

            if (latest != null)
            {
                _context.ReportAudits.Add(new ReportAudit
                {
                    ReportID = latest.ReportID,
                    UserID = userId,
                    Action = "GENERATE",
                    ActionDate = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
            }

            return latest;
        }

        public async Task<List<Report>> GetReportList()
        {
            return await _context.Reports
                .OrderByDescending(r => r.GeneratedDate)
                .ToListAsync();
        }

        public async Task<Report> GetReport(int id)
        {
            return await _context.Reports.FindAsync(id);
        }

        public async Task<List<ReportAudit>> GetSystemAudits()
        {
            return await _context.ReportAudits
                .OrderByDescending(x => x.ActionDate)
                .Take(100)
                .ToListAsync();
        }

        public async Task<bool> DeleteReport(int id)
        {
            var report = await _context.Reports.FindAsync(id);

            if (report == null)
                return false;

            var audits = _context.ReportAudits.Where(x => x.ReportID == id);

            _context.ReportAudits.RemoveRange(audits);
            _context.Reports.Remove(report);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
