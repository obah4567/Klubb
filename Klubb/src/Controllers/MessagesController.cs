using Klubb.src.Domain.DTO.MessagesDto;
using Klubb.src.Domain.IServices;
using Klubb.src.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Klubb.src.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IHubContext<MessageHub> _messageHub;

        public MessagesController(IMessageRepository messageRepository, IHubContext<MessageHub> messageHub)
        {
            _messageRepository = messageRepository;
            _messageHub = messageHub;
        }

        [HttpGet("sent/{userId}")]
        public async Task<IActionResult> GetSentMessagesAsync(int userId)
        {
            try
            {
                var messages = await _messageRepository.GetSentMessagesAsync(userId);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erreur lors de la récupération des messages envoyés : {ex.Message}");
            }
        }

        [HttpGet("received/{userId}")]
        public async Task<IActionResult> GetReceivedMessagesAsync(int userId)
        {
            try
            {
                var messages = await _messageRepository.GetReceivedMessagesAsync(userId);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erreur lors de la récupération des messages reçus : {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendMessageAsync([FromBody] SendMessageDto messageDto)
        {
            try
            {
                var newMessage = _messageRepository.SendMessageAsync(messageDto);
                return Ok(newMessage); // Retournez le message créé ou ses détails si nécessaire
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erreur lors de l'envoi du message : {ex.Message}");
            }
        }

    }
}
