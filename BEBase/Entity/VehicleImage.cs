namespace BEBase.Entity
{
    public class VehicleImage : IEntity
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public Vehicle Vehicle { get; set; }

        public string Url { get; set; }
        public bool IsPrimary { get; set; }
    }

}
