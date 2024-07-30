using Klubb.src.Domain.DTO.MessagesDto;
using Klubb.src.Domain.Models.Messages;

namespace Klubb.src.Domain.IServices
{
    public interface IMessageRepository
    {
        Task SendMessageAsync(SendMessageDto sendMessageDto);
        Task<IEnumerable<Message>> GetReceivedMessagesAsync(int userId);
        Task<IEnumerable<Message>> GetSentMessagesAsync(int userId);
    }
}
