namespace BEBase.Dto
{
    public class ViolationReportDTO
    {
        public int Id { get; set; }
        public string ReporterName { get; set; }
        public string ReportedName { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
    }
}
