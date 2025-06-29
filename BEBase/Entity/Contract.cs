namespace BEBase.Entity
{
    public class Contract : IEntity
    {
        public int Id { get; set; }

        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        public int OwnerId { get; set; }
        public User Owner { get; set; }

        public decimal CommissionRate { get; set; }

        public string Status { get; set; } // draft, sent, signed, expired
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Terms { get; set; }
    }

}
