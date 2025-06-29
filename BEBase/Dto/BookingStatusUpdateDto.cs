namespace BEBase.Dto
{
    public class BookingStatusUpdateDto
    {
        public int BookingId { get; set; }
        public string NewStatus { get; set; } = string.Empty;
    }
}
