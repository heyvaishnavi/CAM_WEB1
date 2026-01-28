using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CAM_WEB1.Data;
using CAM_WEB1.Models;

namespace CAM_WEB1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AccountsController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public AccountsController(ApplicationDbContext context)
		{
			_context = context;
		}

		[HttpPost]
		public async Task<ActionResult<Account>> CreateAccount([FromBody] Account account)
		{
			if (account == null) return BadRequest();

			_context.Accounts.Add(account);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetAccountById), new { id = account.AccountID }, account);
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Account>>> GetAllAccounts()
		{
			return await _context.Accounts.ToListAsync();
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Account>> GetAccountById(int id)
		{
			var account = await _context.Accounts.FindAsync(id);
			if (account == null) return NotFound();
			return account;
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateAccount(int id, [FromBody] Account account)
		{
			if (id != account.AccountID) return BadRequest("ID Mismatch");

			_context.Entry(account).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!_context.Accounts.Any(e => e.AccountID == id)) return NotFound();
				throw;
			}

			return NoContent();
		}

		[HttpPatch("{id}/status")]
		public async Task<IActionResult> ChangeAccountStatus(int id, [FromBody] string newStatus)
		{
			var account = await _context.Accounts.FindAsync(id);
			if (account == null) return NotFound();

			account.Status = newStatus;
			await _context.SaveChangesAsync();

			return Ok(new { Message = $"Account status updated to {newStatus}" });
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAccount(int id)
		{
			var account = await _context.Accounts.FindAsync(id);
			if (account == null) return NotFound();

			_context.Accounts.Remove(account);
			await _context.SaveChangesAsync();

			return NoContent();
		}
	}
}