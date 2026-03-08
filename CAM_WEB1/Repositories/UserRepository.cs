using System.Data;
using Microsoft.Data.SqlClient;
using CAM_WEB1.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CAM_WEB1.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly string _conn;

        public UserRepository(IConfiguration config)
        {
            _conn = config.GetConnectionString("DefaultConnection");
        }

        public DataTable Login(string email)
        {
            using var con = new SqlConnection(_conn);
            using var cmd = new SqlCommand("usp_user_crud", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Action", "LOGIN");
            cmd.Parameters.AddWithValue("@Email", email);

            con.Open();

            DataTable dt = new();
            dt.Load(cmd.ExecuteReader());

            return dt;
        }

        public void SaveRefreshToken(string userID, string refreshToken)
        {
            using var con = new SqlConnection(_conn);
            using var cmd = new SqlCommand("usp_user_crud", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Action", "SAVE_REFRESH");
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@RefreshToken", refreshToken);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void Logout(string refreshToken)
        {
            using var con = new SqlConnection(_conn);
            using var cmd = new SqlCommand("usp_user_crud", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Action", "LOGOUT");
            cmd.Parameters.AddWithValue("@RefreshToken", refreshToken);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public string CreateUser(string name, string email, string passwordHash,
                          string roleID, string branch, string createdBy)
        {
            using var con = new SqlConnection(_conn);
            using var cmd = new SqlCommand("usp_user_crud", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Action", "CREATE");
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
            cmd.Parameters.AddWithValue("@RoleID", roleID);
            cmd.Parameters.AddWithValue("@Branch", branch);
            cmd.Parameters.AddWithValue("@CreatedBy", createdBy);

            con.Open();

            return cmd.ExecuteScalar()?.ToString();
        }


        public DataTable GetUsers(string userID, string roleID, string branch, string status)
        {
            using var con = new SqlConnection(_conn);
            using var cmd = new SqlCommand("usp_user_crud", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Action", "GET");
            cmd.Parameters.AddWithValue("@UserID", (object?)userID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@RoleID", (object?)roleID ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Branch", (object?)branch ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", (object?)status ?? DBNull.Value);

            con.Open();

            DataTable dt = new();
            dt.Load(cmd.ExecuteReader());

            return dt;
        }

        public void UpdateUser(string userID, string name, string branch, string modifiedBy)
        {
            using var con = new SqlConnection(_conn);
            using var cmd = new SqlCommand("usp_user_crud", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Action", "UPDATE");
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Branch", branch);
            cmd.Parameters.AddWithValue("@ModifiedBy", modifiedBy);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void UpdateStatus(string userID, string status, string modifiedBy)
        {
            using var con = new SqlConnection(_conn);
            using var cmd = new SqlCommand("usp_user_crud", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Action", "STATUS");
            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@Status", status);
            cmd.Parameters.AddWithValue("@ModifiedBy", modifiedBy);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public void Audit(string userID, string action, string oldVal, string newVal)
        {
            using var con = new SqlConnection(_conn);
            using var cmd = new SqlCommand("usp_user_audit", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@UserID", userID);
            cmd.Parameters.AddWithValue("@Action", action);
            cmd.Parameters.AddWithValue("@OldValue", (object?)oldVal ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@NewValue", (object?)newVal ?? DBNull.Value);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public DataTable FirstAdmin(string name, string email, string passwordHash, string branch)
        {
            using var con = new SqlConnection(_conn);
            using var cmd = new SqlCommand("usp_user_crud", con);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Action", "FIRST_ADMIN");
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
            cmd.Parameters.AddWithValue("@Branch", branch);

            con.Open();

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            return dt;
        }

    }
}
