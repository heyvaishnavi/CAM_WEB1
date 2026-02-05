using CAM_WEB1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace YourProject.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly string _connectionString;

        public UserController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // REGISTER
        [HttpPost("register")]
        public IActionResult Register(UserCreateRequest model)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("usp_User_Register", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Name", model.Name);
            cmd.Parameters.AddWithValue("@Role", model.Role);
            cmd.Parameters.AddWithValue("@Email", model.Email);
            cmd.Parameters.AddWithValue("@Branch", model.Branch);

            con.Open();
            cmd.ExecuteNonQuery();

            return Ok("User Registered Successfully");
        }

        // LOGIN
        [HttpPost("login")]
        public IActionResult Login(string email)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("usp_User_Login", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Email", email);

            con.Open();
            using SqlDataReader reader = cmd.ExecuteReader();

            if (!reader.Read())
                return Unauthorized("Invalid or Inactive User");

            return Ok(new
            {
                UserId = reader["UserId"],
                Name = reader["Name"],
                Role = reader["Role"],
                Email = reader["Email"],
                Branch = reader["Branch"]
            });
        }

        // GET (ALL / FILTERS)
        // ================= GET (ID / ROLE / BRANCH / STATUS) =================
        [HttpGet]
        public IActionResult GetUsers(
            int? userId,
            string? role,
            string? branch,
            string? status)
        {
            List<User> users = new List<User>();

            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("usp_User_CRUD", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@Action", SqlDbType.NVarChar).Value = "GET";
            cmd.Parameters.Add("@UserId", SqlDbType.Int).Value =
                userId ?? (object)DBNull.Value;
            cmd.Parameters.Add("@Role", SqlDbType.NVarChar).Value =
                role ?? (object)DBNull.Value;
            cmd.Parameters.Add("@Branch", SqlDbType.NVarChar).Value =
                branch ?? (object)DBNull.Value;
            cmd.Parameters.Add("@Status", SqlDbType.NVarChar).Value =
                status ?? (object)DBNull.Value;

            con.Open();
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                users.Add(new User
                {
                    UserID = (int)reader["UserID"],
                    Name = reader["Name"].ToString()!,
                    Role = reader["Role"].ToString()!,
                    Email = reader["Email"].ToString()!,   // returned, but NOT filtered
                    Branch = reader["Branch"]?.ToString(),
                    Status = reader["Status"].ToString()!
                });
            }

            return Ok(users);
        }


        // CREATE
        [HttpPost("create")]
        public IActionResult Create(UserCreateRequest model)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("usp_User_CRUD", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Action", "CREATE");
            cmd.Parameters.AddWithValue("@Name", model.Name);
            cmd.Parameters.AddWithValue("@Role", model.Role);
            cmd.Parameters.AddWithValue("@Email", model.Email);
            cmd.Parameters.AddWithValue("@Branch", model.Branch);

            con.Open();
            cmd.ExecuteNonQuery();

            return Ok("User Created");
        }

        // UPDATE (PUT)
        [HttpPut("{id}")]
        public IActionResult Update(int id, UserCreateRequest model)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("usp_User_CRUD", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Action", "UPDATE");
            cmd.Parameters.AddWithValue("@UserId", id);
            cmd.Parameters.AddWithValue("@Name", model.Name);
            cmd.Parameters.AddWithValue("@Role", model.Role);
            cmd.Parameters.AddWithValue("@Email", model.Email);
            cmd.Parameters.AddWithValue("@Branch", model.Branch);

            con.Open();
            cmd.ExecuteNonQuery();

            return Ok("User Updated");
        }

        // PATCH (STATUS TOGGLE)
        [HttpPatch("status/{id}")]
        public IActionResult ToggleStatus(int id)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("usp_User_CRUD", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Action", "PATCH");
            cmd.Parameters.AddWithValue("@UserId", id);

            con.Open();
            cmd.ExecuteNonQuery();

            return Ok("User Status Updated");
        }

        // DELETE
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            using SqlConnection con = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("usp_User_CRUD", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Action", "DELETE");
            cmd.Parameters.AddWithValue("@UserId", id);

            con.Open();
            cmd.ExecuteNonQuery();

            return Ok("User Deleted");
        }
        
    }
    public class UserCreateRequest
    {
        public string Name { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Branch { get; set; }
    }
}