using CAM_WEB1.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace CAM_WEB1.Controllers
{
    [ApiController]
    [Route("api/v1/reports")]
    [Authorize(Roles = "Manager,Admin")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _service;

        public ReportsController(IReportService service)
        {
            _service = service;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateSnapshot(DateTime from, DateTime to, string branch = "Global")
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var report = await _service.GenerateSnapshot(from, to, branch, userId);

            return Ok(report);
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetReportList()
        {
            return Ok(await _service.GetReportList());
        }

        [HttpGet("{id}/export")]
        public async Task<IActionResult> ExportReport(int id)
        {
            var report = await _service.GetReport(id);

            if (report == null)
                return NotFound();

            var csv = new StringBuilder();

            csv.AppendLine("ReportID,Branch,TotalTransactions,HighValueCount,AccountGrowthRate,GeneratedDate");

            csv.AppendLine($"{report.ReportID},{report.Branch},{report.TotalTransactions},{report.HighValueCount},{report.AccountGrowthRate}%,{report.GeneratedDate}");

            return File(Encoding.UTF8.GetBytes(csv.ToString()), "text/csv", $"Report_{id}.csv");
        }

        [HttpGet("system-audits")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSystemAudits()
        {
            return Ok(await _service.GetSystemAudits());
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteReport(int id)
        {
            var deleted = await _service.DeleteReport(id);

            if (!deleted)
                return NotFound();

            return Ok(new { message = "Report deleted successfully" });
        }
    }
}
