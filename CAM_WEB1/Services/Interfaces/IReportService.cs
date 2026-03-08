using CAM_WEB1.Models;

namespace CAM_WEB1.Services.Interfaces
{
    public interface IReportService
    {
        Task<Report> GenerateSnapshot(DateTime from, DateTime to, string branch, string userId);

        Task<List<Report>> GetReportList();

        Task<Report> GetReport(int id);

        Task<List<ReportAudit>> GetSystemAudits();

        Task<bool> DeleteReport(int id);
    }
}
