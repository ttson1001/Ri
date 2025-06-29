namespace BEBase.Entity
{
    public class VehicleDocument : IEntity
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        public string Type { get; set; }
        public string Url { get; set; }
    }
}
