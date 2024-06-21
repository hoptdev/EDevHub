using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace ChatService.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        public ChatHub()
        {
        }

        /* public Task Receive(string message)
{
    return Clients.User("18").SendAsync("Send", message);
} */

        public async Task Receive(string message, string to)
        {
            var userName = Context.User.Identity.Name;

            await Clients.User(to).SendAsync("Receive", message);
        }
    }
}
