using System.ComponentModel.DataAnnotations;

namespace CAM_WEB1.DTOs
{
    public class TransactionCreateDTO
    {
        [Required]
        public string FromAccountID { get; set; }

        public string? ToAccountID { get; set; }

        [Required]
        public string Type { get; set; }

        [Range(1, double.MaxValue)]
        public decimal Amount { get; set; }
    }
}
