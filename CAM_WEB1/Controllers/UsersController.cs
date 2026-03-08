using CAM_WEB1.DTO;
using CAM_WEB1.Helpers;
using CAM_WEB1.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CAM_WEB1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        private string GetUserID()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        //------------------------------------------------
        // 1 FIRST ADMIN
        //------------------------------------------------
        [HttpPost("first-admin")]
        [AllowAnonymous]
        public IActionResult FirstAdmin(UserCreateRequest req)
        {
            try
            {
                var dt = _service.FirstAdmin(req);

                var result = DataTableHelper.ToList(dt);

                if (result.Count > 0 && result[0]["Result"].ToString() == "ADMIN_EXISTS")
                    return Conflict("First admin already exists");

                return Ok("First admin created successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //------------------------------------------------
        // 2 LOGIN
        //------------------------------------------------
        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login(LoginRequest1 req)
        {
            var result = _service.Login(req);

            if (result == null)
                return Unauthorized("Invalid credentials");

            return Ok(result);
        }

        //------------------------------------------------
        // 3 LOGOUT
        //------------------------------------------------
        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout(RefreshTokenRequest req)
        {
            string userID = GetUserID();

            _service.Logout(req, userID);

            return Ok("Logged out");
        }

        //------------------------------------------------
        // 4 CREATE USER
        //------------------------------------------------
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(UserCreateRequest req)
        {
            try
            {
                var loggedUserID = GetUserID();

                _service.CreateUser(req, loggedUserID);

                return Ok("User created successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //------------------------------------------------
        // 5 GET USERS
        //------------------------------------------------
        [HttpGet]
        [Authorize]
        [HttpGet]
        public IActionResult Get(
    string? id = null,
    string? role = null,
    string? branch = null,
    string? status = null)
        {
            var dt = _service.GetUsers(id, role, branch, status);

            var result = DataTableHelper.ToList(dt);

            return Ok(result);
        }


        //------------------------------------------------
        // 6 UPDATE USER
        //------------------------------------------------
        [HttpPut("{id}")]
        [Authorize ]
        public IActionResult Update(string id, UserUpdateRequest req)
        {
             try
            {
                string loggedUserID = GetUserID();
                bool isAdmin = User.IsInRole("Admin");

                _service.UpdateUser(id, req, loggedUserID, isAdmin);

                return Ok("Updated");
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid(); // HTTP 403
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //------------------------------------------------
        // 7 UPDATE STATUS
        //------------------------------------------------
        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateStatus(string id, string status)
        {
            string modifiedBy = GetUserID();

            _service.UpdateStatus(id, status, modifiedBy);

            return Ok("Status updated");
        }
    }
}
