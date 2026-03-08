
using CAM_WEB1.DTOs;
using CAM_WEB1.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CAM_WEB1.Controllers
{
    [ApiController]
    [Route("api/transactions")]
    [Authorize(Roles = "Officer,Manager")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _service;

        public TransactionsController(ITransactionService service)
        {
            _service = service;
        }
        [HttpPost("initiate")]
        [Authorize(Roles = "Officer")]
        public async Task<IActionResult> Create(TransactionCreateDTO request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId == null)
                    return Unauthorized("Invalid token");

                await _service.CreateTransaction(request, userId);

                return Ok(new
                {
                    message = "Transaction completed successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var data = await _service.GetById(id);

            if (data == null)
                return NotFound("Transaction not found");

            return Ok(data);
        }
    }
}
