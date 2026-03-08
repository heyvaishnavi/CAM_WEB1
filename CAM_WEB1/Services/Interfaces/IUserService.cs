using CAM_WEB1.DTO;
using Microsoft.AspNetCore.Identity.Data;
using System.Data;

namespace CAM_WEB1.Services.Interfaces
{
    public interface IUserService
    {
        DataTable FirstAdmin(UserCreateRequest req);

        object Login(LoginRequest1 req);

        void Logout(RefreshTokenRequest req, string userID);

        void CreateUser(UserCreateRequest req, string createdBy);

        DataTable GetUsers(string id, string role, string branch, string status);

        void UpdateUser(string id, UserUpdateRequest req, string loggedUserID, bool isAdmin);

        void UpdateStatus(string id, string status, string modifiedBy);
    }
}
