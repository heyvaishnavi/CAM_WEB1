using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAM_WEB1.Models
{
    [Table("t_ReportAudit")]
    public class ReportAudit
    {
        [Key]
        public int AuditID { get; set; }

        public int ReportID { get; set; }

        public string UserID { get; set; }

        public string Action { get; set; }

        public DateTime ActionDate { get; set; }
    }
}
