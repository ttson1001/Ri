using BEBase.Entity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BEBase.Database
{
    public class BaseDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleDocument> VehicleDocuments { get; set; }
        public DbSet<VehicleImage> VehicleImages { get; set; }
        public DbSet<ViolationReport> ViolationReports { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public BaseDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
