using CAM_WEB1.DTO;
using CAM_WEB1.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CAM_WEB1.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _service;

        public AccountsController(IAccountService service)
        {
            _service = service;
        }

        private string GetUserID()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [HttpPost("create")]
        [Authorize(Roles = "Officer")]
        public async Task<IActionResult> CreateAccount(AccountCreateRequest req)
        {
            var result = await _service.CreateAccount(req, GetUserID());

            return Ok(new
            {
                message = "Account successfully created",
                data = result.FirstOrDefault()
            });
        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = "Officer")]
        public async Task<IActionResult> UpdateAccount(string id, AccountUpdateRequest req)
        {
            var result = await _service.UpdateAccount(id, req, GetUserID());

            if (!result.Any())
                return NotFound("Account not found");

            return Ok(new
            {
                message = "Account details updated successfully",
                data = result.FirstOrDefault()
            });
        }

        [HttpPut("close/{id}")]
        [Authorize(Roles = "Officer")]
        public async Task<IActionResult> CloseAccount(string id)
        {
            await _service.CloseAccount(id, GetUserID());

            return Ok(new { message = "Account status updated to Closed" });
        }

        [HttpGet("details/{id}")]
        [Authorize(Roles = "Officer,Manager,Admin")]
        public async Task<IActionResult> GetAccountDetail(string id)
        {
            var result = await _service.GetAccountById(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("all")]
        [Authorize(Roles = "Officer,Manager,Admin")]
        public async Task<IActionResult> ListAllAccounts()
        {
            return Ok(await _service.GetAllAccounts());
        }
    }
}
