namespace BEBase.Dto
{
    public class DashboardOverviewDto
    {
        public int TotalVehicles { get; set; }
        public int TotalUsers { get; set; }
        public decimal TotalCommission { get; set; }
        public int TotalBookings { get; set; }
        public int ActiveVehicles { get; set; }
        public int ActiveUsers { get; set; }
        public int PendingVehicles { get; set; }
        public int PendingReports { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal MonthlyRevenue { get; set; }
        public int MonthlyBookings { get; set; }
        public decimal CommissionRate { get; set; }
    }

    public class VehicleStatisticsDto
    {
        public int Total { get; set; }
        public int Active { get; set; }
        public int Pending { get; set; }
        public int Blocked { get; set; }
        public int ApprovedThisMonth { get; set; }
    }

    public class UserStatisticsDto
    {
        public int Total { get; set; }
        public int Active { get; set; }
        public int Blocked { get; set; }
        public int NewThisMonth { get; set; }
        public int Renters { get; set; }
        public int Owners { get; set; }
        public int Staff { get; set; }
        public int Admins { get; set; }
    }

    public class RevenueStatisticsDto
    {
        public decimal TotalRevenue { get; set; }
        public decimal TotalCommission { get; set; }
        public decimal MonthlyRevenue { get; set; }
        public decimal MonthlyCommission { get; set; }
        public decimal CommissionRate { get; set; }
        public int TotalBookings { get; set; }
        public int CompletedBookings { get; set; }
        public int MonthlyBookings { get; set; }
    }
}
