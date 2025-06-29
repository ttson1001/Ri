namespace BEBase.Dto
{
    public class BookingCreateDto
    {
        public int VehicleId { get; set; }
        public int RenterId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public TimeSpan PickupTime { get; set; }
        public TimeSpan ReturnTime { get; set; }

        public decimal ServiceFee { get; set; }
        public decimal InsuranceFee { get; set; }
    }

}
