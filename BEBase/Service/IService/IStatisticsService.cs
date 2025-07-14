using BEBase.Dto;

namespace BEBase.Service.IService
{
    public interface IStatisticsService
    {
        Task<DashboardOverviewDto> GetDashboardOverviewAsync();
        Task<int> GetTotalVehiclesCountAsync();
        Task<int> GetTotalUsersCountAsync();
        Task<decimal> GetTotalCommissionAsync();
        Task<VehicleStatisticsDto> GetVehicleStatisticsAsync();
        Task<UserStatisticsDto> GetUserStatisticsAsync();
        Task<RevenueStatisticsDto> GetRevenueStatisticsAsync();
        Task<int> GetTotalBookingsCountAsync();
        Task<int> GetActiveVehiclesCountAsync();
        Task<int> GetActiveUsersCountAsync();
        Task<int> GetPendingVehiclesCountAsync();
        Task<int> GetPendingReportsCountAsync();
        Task<decimal> GetMonthlyRevenueAsync();
        Task<int> GetMonthlyBookingsCountAsync();
    }
}
