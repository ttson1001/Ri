namespace BEBase.Entity
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; } // or ProviderId
        public string Role { get; set; } // renter / owner / admin

        // Profile fields
        public string Name { get; set; }
        public string AvatarUrl { get; set; }
        public string Phone { get; set; }
        public DateTime JoinDate { get; set; }
        public float? Rating { get; set; }

        public bool IsBlocked { get; set; } = false;
        public DateTime? LastActiveDate { get; set; }

        public ICollection<Vehicle> Vehicles { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}
