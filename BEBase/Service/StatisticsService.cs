using BEBase.Database;
using BEBase.Dto;
using BEBase.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace BEBase.Service
{
    public class StatisticsService : IStatisticsService
    {
        private readonly BaseDbContext _context;

        public StatisticsService(BaseDbContext context)
        {
            _context = context;
        }

        public async Task<DashboardOverviewDto> GetDashboardOverviewAsync()
        {
            var totalVehicles = await GetTotalVehiclesCountAsync();
            var totalUsers = await GetTotalUsersCountAsync();
            var totalCommission = await GetTotalCommissionAsync();
            var totalBookings = await GetTotalBookingsCountAsync();
            var activeVehicles = await GetActiveVehiclesCountAsync();
            var activeUsers = await GetActiveUsersCountAsync();
            var pendingVehicles = await GetPendingVehiclesCountAsync();
            var pendingReports = await GetPendingReportsCountAsync();
            var monthlyRevenue = await GetMonthlyRevenueAsync();
            var monthlyBookings = await GetMonthlyBookingsCountAsync();

            // Calculate total revenue from all completed bookings
            var totalRevenue = await _context.Bookings
                .Where(b => b.Status.ToLower() == "completed")
                .SumAsync(b => b.TotalAmount);

            // Get commission rate from settings
            var commissionRate = await GetCommissionRateAsync();

            return new DashboardOverviewDto
            {
                TotalVehicles = totalVehicles,
                TotalUsers = totalUsers,
                TotalCommission = totalCommission,
                TotalBookings = totalBookings,
                ActiveVehicles = activeVehicles,
                ActiveUsers = activeUsers,
                PendingVehicles = pendingVehicles,
                PendingReports = pendingReports,
                TotalRevenue = totalRevenue,
                MonthlyRevenue = monthlyRevenue,
                MonthlyBookings = monthlyBookings,
                CommissionRate = commissionRate
            };
        }

        public async Task<int> GetTotalVehiclesCountAsync()
        {
            return await _context.Vehicles.CountAsync();
        }

        public async Task<int> GetTotalUsersCountAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<decimal> GetTotalCommissionAsync()
        {
            var commissionRate = await GetCommissionRateAsync();
            var totalRevenue = await _context.Bookings
                .Where(b => b.Status.ToLower() == "completed")
                .SumAsync(b => b.TotalAmount);

            return totalRevenue * commissionRate;
        }

        public async Task<VehicleStatisticsDto> GetVehicleStatisticsAsync()
        {
            var total = await _context.Vehicles.CountAsync();
            var active = await _context.Vehicles.CountAsync(v => v.Status.ToLower() == "approved");
            var pending = await _context.Vehicles.CountAsync(v => v.Status.ToLower() == "pending");
            var blocked = await _context.Vehicles.CountAsync(v => v.Status.ToLower() == "blocked");

            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            var approvedThisMonth = await _context.Vehicles
                .CountAsync(v => v.Status.ToLower() == "approved" && 
                           v.CreatedAt.Month == currentMonth && 
                           v.CreatedAt.Year == currentYear);

            return new VehicleStatisticsDto
            {
                Total = total,
                Active = active,
                Pending = pending,
                Blocked = blocked,
                ApprovedThisMonth = approvedThisMonth
            };
        }

        public async Task<UserStatisticsDto> GetUserStatisticsAsync()
        {
            var total = await _context.Users.CountAsync();
            var active = await _context.Users.CountAsync(u => !u.IsBlocked);
            var blocked = await _context.Users.CountAsync(u => u.IsBlocked);

            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;
            var newThisMonth = await _context.Users
                .CountAsync(u => u.JoinDate.Month == currentMonth && u.JoinDate.Year == currentYear);

            var renters = await _context.Users.CountAsync(u => u.Role.ToLower() == "renter");
            var owners = await _context.Users.CountAsync(u => u.Role.ToLower() == "owner");
            var staff = await _context.Users.CountAsync(u => u.Role.ToLower() == "staff");
            var admins = await _context.Users.CountAsync(u => u.Role.ToLower() == "admin");

            return new UserStatisticsDto
            {
                Total = total,
                Active = active,
                Blocked = blocked,
                NewThisMonth = newThisMonth,
                Renters = renters,
                Owners = owners,
                Staff = staff,
                Admins = admins
            };
        }

        public async Task<RevenueStatisticsDto> GetRevenueStatisticsAsync()
        {
            var totalRevenue = await _context.Bookings
                .Where(b => b.Status.ToLower() == "completed")
                .SumAsync(b => b.TotalAmount);

            var commissionRate = await GetCommissionRateAsync();
            var totalCommission = totalRevenue * commissionRate;

            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            var monthlyRevenue = await _context.Bookings
                .Where(b => b.Status.ToLower() == "completed" &&
                           b.StartDate.Month == currentMonth &&
                           b.StartDate.Year == currentYear)
                .SumAsync(b => b.TotalAmount);

            var monthlyCommission = monthlyRevenue * commissionRate;

            var totalBookings = await _context.Bookings.CountAsync();
            var completedBookings = await _context.Bookings.CountAsync(b => b.Status.ToLower() == "completed");

            var monthlyBookings = await _context.Bookings
                .CountAsync(b => b.StartDate.Month == currentMonth &&
                           b.StartDate.Year == currentYear);

            return new RevenueStatisticsDto
            {
                TotalRevenue = totalRevenue,
                TotalCommission = totalCommission,
                MonthlyRevenue = monthlyRevenue,
                MonthlyCommission = monthlyCommission,
                CommissionRate = commissionRate,
                TotalBookings = totalBookings,
                CompletedBookings = completedBookings,
                MonthlyBookings = monthlyBookings
            };
        }

        public async Task<int> GetTotalBookingsCountAsync()
        {
            return await _context.Bookings.CountAsync();
        }

        public async Task<int> GetActiveVehiclesCountAsync()
        {
            return await _context.Vehicles.CountAsync(v => v.Status.ToLower() == "approved");
        }

        public async Task<int> GetActiveUsersCountAsync()
        {
            return await _context.Users.CountAsync(u => !u.IsBlocked);
        }

        public async Task<int> GetPendingVehiclesCountAsync()
        {
            return await _context.Vehicles.CountAsync(v => v.Status.ToLower() == "pending");
        }

        public async Task<int> GetPendingReportsCountAsync()
        {
            return await _context.ViolationReports.CountAsync(r => r.Status.ToLower() == "pending");
        }

        public async Task<decimal> GetMonthlyRevenueAsync()
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            return await _context.Bookings
                .Where(b => b.Status.ToLower() == "completed" &&
                           b.StartDate.Month == currentMonth &&
                           b.StartDate.Year == currentYear)
                .SumAsync(b => b.TotalAmount);
        }

        public async Task<int> GetMonthlyBookingsCountAsync()
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            return await _context.Bookings
                .CountAsync(b => b.StartDate.Month == currentMonth &&
                           b.StartDate.Year == currentYear);
        }

        private async Task<decimal> GetCommissionRateAsync()
        {
            var commissionSetting = await _context.Settings
                .FirstOrDefaultAsync(s => s.Key.ToLower() == "commission");

            if (commissionSetting != null && decimal.TryParse(commissionSetting.Value, out var rate))
            {
                return rate;
            }

            // Default commission rate if not found in settings
            return 0.20m; // 20%
        }
    }
}
