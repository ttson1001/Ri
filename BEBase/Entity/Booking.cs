namespace BEBase.Entity
{
    public class Booking: IEntity
    {
        public int Id { get; set; }

        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        public int RenterId { get; set; }
        public User Renter { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public TimeSpan PickupTime { get; set; }
        public TimeSpan ReturnTime { get; set; }

        public decimal ServiceFee { get; set; }
        public decimal InsuranceFee { get; set; }
        public decimal DepositAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } // requested, approved, active, completed, cancelled

        public ICollection<Review> Reviews { get; set; }
        public ICollection<Message> Messages { get; set; }
    }

}
