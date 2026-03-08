using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAM_WEB1.Models
{
    [Table("t_Transaction")]
    public class Transaction
    {
        [Key]
        public string TransactionID { get; set; }

        public string FromAccountID { get; set; }

        public string? ToAccountID { get; set; }

        public string Type { get; set; }

        public decimal Amount { get; set; }

        public string Status { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string? ModifiedBy { get; set; }

       
    }
}
