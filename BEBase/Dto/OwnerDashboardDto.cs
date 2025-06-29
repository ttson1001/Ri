namespace BEBase.Dto
{
    public class OwnerDashboardDto
    {
        public int TotalVehicles { get; set; }
        public int ActiveVehicles { get; set; }
        public int NewRequests { get; set; }
        public decimal MonthlyRevenue { get; set; }
        public decimal TotalRevenue { get; set; }
    }

}
