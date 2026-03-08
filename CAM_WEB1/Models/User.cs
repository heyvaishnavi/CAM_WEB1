namespace CAM_WEB1.Models
{
    public class User
    {
        public string UserID { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string RoleID { get; set; }

        public string Branch { get; set; }

        public string Status { get; set; }

        public string RefreshToken { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
