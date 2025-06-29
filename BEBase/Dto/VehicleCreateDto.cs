namespace BEBase.Dto
{
    public class VehicleCreateDto
    {
        public int OwnerId { get; set; }

        public string Name { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; } 
        public string Type { get; set; }

        public string LicensePlate { get; set; }
        public string Description { get; set; }

        public decimal PricePerDay { get; set; }
        public decimal DepositAmount { get; set; }
    }


}
