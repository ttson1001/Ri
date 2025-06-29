namespace BEBase.Dto
{
    public class MessageDto
    {
        public int BookingId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
    }

}
