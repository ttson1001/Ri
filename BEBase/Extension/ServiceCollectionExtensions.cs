using BEBase.Repository;
using BEBase.Service;
using BEBase.Service.IService;
using BEBase.Services.Implementations;

namespace BEBase.Extension
{
    public static class ServiceCollectionExtensions
    {
        public static void Register(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepo<>), typeof(Repo<>));
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAdminUserService, AdminUserService>();
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IViolationReportService, ViolationReportService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<ISettingService, SettingService>();
            services.AddScoped<IReviewService, ReviewService>();
        }
    }
}
