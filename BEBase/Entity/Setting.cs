namespace BEBase.Entity
{
    public class Setting : IEntity
    {
        public int Id { get; set; }
        public string Key { get; set; } = default!;
        public string Value { get; set; } = default!;
    }
}
