using BCrypt.Net;
using CAM_WEB1.DTO;
using CAM_WEB1.Helpers;
using CAM_WEB1.Models;
using CAM_WEB1.Repositories.Interfaces;
using CAM_WEB1.Services.Interfaces;
using System.Data;
using System.Reflection;

namespace CAM_WEB1.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly JwtHelper _jwt;

        public UserService(IUserRepository repo, JwtHelper jwt)
        {
            _repo = repo;
            _jwt = jwt;
        }

        public DataTable FirstAdmin(UserCreateRequest req)
        {
            return _repo.FirstAdmin(
                req.Name,
                req.Email,
                BCrypt.Net.BCrypt.HashPassword(req.Password),
                req.Branch
            );
        }



        public object Login(LoginRequest1 req)
        {
            var dt = _repo.Login(req.Email);

            if (dt.Rows.Count == 0)
                return null;

            var row = dt.Rows[0];

            string hash = row["PasswordHash"].ToString();

            if (!BCrypt.Net.BCrypt.Verify(req.Password, hash))
                return null;

            string userID = row["UserID"].ToString();
            string role = row["RoleName"].ToString();

            string accessToken = _jwt.GenerateJwt(userID, role);
            string refreshToken = Guid.NewGuid().ToString();

            _repo.SaveRefreshToken(userID, refreshToken);

            _repo.Audit(userID, "LOGIN", null, "SUCCESS");

            return new
            {
                accessToken,
                refreshToken
            };
        }

        public void Logout(RefreshTokenRequest req, string userID)
        {
            _repo.Logout(req.RefreshToken);

            _repo.Audit(userID, "LOGOUT", null, "SUCCESS");
        }

        public void CreateUser(UserCreateRequest req, string loggedUserID)
        {
            var result = _repo.CreateUser(
                req.Name,
                req.Email,
                BCrypt.Net.BCrypt.HashPassword(req.Password),
                req.RoleID,
                req.Branch,
                loggedUserID
            );

            if (result == "INVALID_EMAIL")
                throw new Exception("Only gmail.com email allowed");

            if (result == "EMAIL_EXISTS")
                throw new Exception("Email already exists");

            if (result != "SUCCESS")
                throw new Exception("User creation failed");

            _repo.Audit(loggedUserID, "CREATE_USER", null, req.Email);
        }
       


        public DataTable GetUsers(string id, string role, string branch, string status)
        {
            return _repo.GetUsers(id, role, branch, status);
        }

        public void UpdateUser(string id, UserUpdateRequest req, string loggedUserID, bool isAdmin)
        {
            if (!isAdmin && loggedUserID != id)
                throw new UnauthorizedAccessException();

            _repo.UpdateUser(id, req.Name, req.Branch, loggedUserID);

            _repo.Audit(loggedUserID, "UPDATE_USER", null, id);
        }

        public void UpdateStatus(string id, string status, string modifiedBy)
        {
            _repo.UpdateStatus(id, status, modifiedBy);

            _repo.Audit(modifiedBy, "UPDATE_STATUS", null, status);
        }
    }
}
