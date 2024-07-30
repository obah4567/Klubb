using Klubb.src.Domain.DTO.MessagesDto;
using Klubb.src.Domain.IServices;
using Klubb.src.Domain.Models.Messages;
using Klubb.src.Infrastructure.Dbontext;
using Microsoft.EntityFrameworkCore;

namespace Klubb.src.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _dataContext;

        public MessageRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Message> GetReceivedMessageAsync(int messageId)
        {
            return await _dataContext.Messages
                .Where(m => m.SenderId == messageId)
                .Include(m => m.Sender)
                .SingleOrDefaultAsync();
        }

        public async Task<Message> GetSentMessageAsync(int messageId)
        {
            return await _dataContext.Messages
                .Where(m => m.SenderId == messageId)
                .Include(m => m.Recipient)
                .SingleOrDefaultAsync();
        }

        public async Task SendMessageAsync(SendMessageDto sendMessageDto)
        {
            var message = new Message
            {
                Content = sendMessageDto.Content,
                SenderId = sendMessageDto.SenderId,
                RecipientId = sendMessageDto.RecipientId,
                SendDate = DateTime.Now
            };

            _dataContext.Messages.Add(message);
            await _dataContext.SaveChangesAsync();
        }
    }
}
