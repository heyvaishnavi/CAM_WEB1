namespace CAM_WEB1.DTO
{
    public class AccountCreateRequest
    {
        public string CustomerName { get; set; }


        public string AccountType { get; set; }

        public decimal Balance { get; set; }

        public string Branch { get; set; }
    }
}

