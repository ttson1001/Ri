namespace BEBase.Dto
{
    public class BookingDto
    {
        public int Id { get; set; }

        public string VehicleName { get; set; }

        public string RenterName { get; set; }
        public string RenterEmail { get; set; }
        public int? RenterId { get; set; }

        public int? OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string OwnerEmail { get; set; }
        public string Image { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan PickupTime { get; set; }
        public TimeSpan ReturnTime { get; set; }

        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
    }


}
