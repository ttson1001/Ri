namespace BEBase.Entity
{
    public class ViolationReport : IEntity
    {
        public int Id { get; set; }

        public int ReporterId { get; set; }
        public User Reporter { get; set; }

        public int ReportedId { get; set; }
        public User Reported { get; set; }

        public string Content { get; set; }
        public DateTime Time { get; set; }
        public string Status { get; set; }  // "pending" or "resolved"
        public string Type { get; set; }  // "harassment", "fraud", "damage", or "other"
    }
}
