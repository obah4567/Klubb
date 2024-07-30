namespace Klubb.src.Domain.DTO.EventDTO
{
    public class CreateEventDto
    {
        public string Title {  get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public EventType Type { get; set; }
        public int CreatorId { get; set; }
    }
}
