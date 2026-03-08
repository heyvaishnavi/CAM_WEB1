using System.ComponentModel.DataAnnotations;

namespace CAM_WEB1.Models
{
    public class Account
    {
        [Key]
        public string AccountID { get; set; }

        public string CustomerID { get; set; }

        public string CustomerName { get; set; }

        public string AccountType { get; set; }

        public decimal Balance { get; set; }

        public string Status { get; set; }

        public string Branch { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
