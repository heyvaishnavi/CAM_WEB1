using CAM_WEB1.DTO;
using CAM_WEB1.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Claims;

namespace CAM_WEB1.Controllers
{
    [ApiController]
    [Route("api/approvals")]
    [Authorize(Roles = "Manager")]
    public class ApprovalController : ControllerBase
    {
        private readonly IApprovalService _service;

        public ApprovalController(IApprovalService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _service.GetById(id);

            if (data == null)
                return NotFound("Approval not found");

            return Ok(data);
        }

        [HttpPost("{id}/decision")]
        public async Task<IActionResult> SubmitDecision(int id, [FromBody] ApprovalDecisionDTO request)
        {
            if (request == null || string.IsNullOrEmpty(request.Decision))
                return BadRequest(new { message = "Invalid decision request" });

            try
            {
                var claim = User.FindFirst(ClaimTypes.NameIdentifier);

                if (claim == null)
                    return Unauthorized(new { message = "Invalid token" });

                string reviewerId = claim.Value;

                await _service.SubmitDecision(id, request, reviewerId);

                return Ok(new
                {
                    message = "Decision submitted successfully"
                });
            }
            catch (SqlException ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Internal server error",
                    error = ex.Message
                });
            }
        }

    }
}
