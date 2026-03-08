using CAM_WEB1.Models;
using CAM_WEB1.Repositories.Interfaces;
using CAM_WEB1.Services.Interfaces;

namespace CAM_WEB1.Services.Implementations
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _repo;

        public ReportService(IReportRepository repo)
        {
            _repo = repo;
        }

        public async Task<Report> GenerateSnapshot(DateTime from, DateTime to, string branch, string userId)
        {
            if (from > to)
                throw new Exception("From date cannot be greater than To date");

            return await _repo.GenerateSnapshot(from, to, branch, userId);
        }

        public async Task<List<Report>> GetReportList()
        {
            return await _repo.GetReportList();
        }

        public async Task<Report> GetReport(int id)
        {
            return await _repo.GetReport(id);
        }

        public async Task<List<ReportAudit>> GetSystemAudits()
        {
            return await _repo.GetSystemAudits();
        }

        public async Task<bool> DeleteReport(int id)
        {
            return await _repo.DeleteReport(id);
        }
    }
}
