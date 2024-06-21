using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ChatService.Hubs
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public virtual string? GetUserId(HubConnectionContext connection)
        {
            var id = connection.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            return id;
        }
    }
}
