using ChatService.Database;
using ChatService.Models;
using Microsoft.EntityFrameworkCore;
using UserService.Helpers;

namespace ChatService.Helpers
{
    public class MessageHelper : Helper<Message>
    {
        public MessageHelper(IDbRepository dbRepository) : base(dbRepository)
        {
        }

        public async Task<List<Message>> GetMessagesAsync(int chatId)
        {
            return await dbRepository.Get<Message>(x => x.ChatId == chatId).ToListAsync();
        }
    }
}
