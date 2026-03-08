namespace CAM_WEB1.DTO
{
    public class ReportGenerateDTO
    {
        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public string Branch { get; set; } = "Global";
    }
}
