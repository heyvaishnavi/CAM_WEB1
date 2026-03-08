using System.Data;

namespace CAM_WEB1.Repositories.Interfaces
{
    public interface IUserRepository
    {
        DataTable FirstAdmin(string name, string email, string passwordHash, string branch);

        DataTable Login(string email);

        void SaveRefreshToken(string userID, string refreshToken);

        void Logout(string refreshToken);

        string CreateUser(string name, string email, string passwordHash, string roleID, string branch, string createdBy);

        DataTable GetUsers(string userID, string roleID, string branch, string status);

        void UpdateUser(string userID, string name, string branch, string modifiedBy);

        void UpdateStatus(string userID, string status, string modifiedBy);

        void Audit(string userID, string action, string oldVal, string newVal);
    }
}
