namespace BEBase.Entity
{
    public class Vehicle : IEntity
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }

        public string Name { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Type { get; set; }

        public string LicensePlate { get; set; }
        public string Description { get; set; }
        public decimal PricePerDay { get; set; }
        public string LicenseRequired { get; set; }
        public int MinAge { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<VehicleImage> Images { get; set; }
        public ICollection<VehicleDocument> Documents { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }


}
