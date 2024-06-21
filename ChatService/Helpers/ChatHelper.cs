using ChatService.Database;
using ChatService.Models;
using Microsoft.EntityFrameworkCore;
using UserService.Helpers;

namespace ChatService.Helpers
{
    public class ChatHelper : Helper<Chat>
    {
        public ChatHelper(IDbRepository dbRepository) : base(dbRepository)
        {
        }

        public async Task<List<Chat>> GetChatAsync(int userId)
        {
            return await dbRepository.Get<Chat>(x => x.User1Id == userId || x.User2Id == userId).ToListAsync();
        }

        public async Task<int> TryCreate(int u1Id, int u2Id)
        {
            var chat = await dbRepository.Get<Chat>(x => (x.User1Id == u1Id && x.User2Id == u2Id) || (x.User1Id == u2Id && x.User2Id == u1Id)).FirstOrDefaultAsync();

            if (chat is null)
            {
                chat = new Chat(u1Id, u2Id);

                return await Add(chat);
            }

            return chat.Id;
        }
    }
}
