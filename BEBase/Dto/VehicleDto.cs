namespace BEBase.Dto
{
    public class VehicleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Type { get; set; }
        public string LicensePlate { get; set; }
        public string Description { get; set; }

        public decimal PricePerDay { get; set; }
        public decimal DepositAmount { get; set; }

        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int OwnerId { get; set; }
        public string OwnerName { get; set; }
        public string OwnerEmail { get; set; }

        public List<string> ImageUrls { get; set; }
        public List<string> DocumentUrls { get; set; }
    }


}
