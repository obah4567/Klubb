namespace Klubb.src.Domain.DTO.MessagesDto
{
    public class SendMessageDto
    {
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public string Content { get; set; }
    }
}
