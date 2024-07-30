
using Microsoft.AspNetCore.SignalR;

namespace Klubb.src.Infrastructure.Repositories
{
    public class MessageHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceivedMessage", user, message);
        }
    }
}
