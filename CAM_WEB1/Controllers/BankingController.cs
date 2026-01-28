using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CAM_WEB1.Data;
using CAM_WEB1.Models; // This line fixes the 'Transaction' not found error

namespace CAM_WEB1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BankingController : ControllerBase
	{
		// Now you can use 'Transaction' in your API methods
		[HttpPost("log")]
		public async Task<IActionResult> LogTransaction([FromBody] Transaction transaction)
		{
			return Ok();
		}
	}
}