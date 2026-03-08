namespace CAM_WEB1.DTO
{
    public class UserCreateRequest
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string RoleID { get; set; }

        public string Branch { get; set; }
    }
}
