using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAM_WEB1.Models
{
    [Table("t_Reports")]
    public class Report
    {
        [Key]
        public int ReportID { get; set; }

        public string Branch { get; set; }

        public int TotalTransactions { get; set; }

        public int HighValueCount { get; set; }

        public decimal AccountGrowthRate { get; set; }

        public DateTime GeneratedDate { get; set; }
    }
}
