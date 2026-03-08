namespace CAM_WEB1.Models
{
    public class Approval
    {
        public int ApprovalID { get; set; }

        public string TransactionID { get; set; }

        public string? ReviewerID { get; set; }

        public string Decision { get; set; }

        public string Comments { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
