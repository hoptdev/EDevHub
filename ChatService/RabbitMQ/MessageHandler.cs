using ChatService.Database;
using ChatService.Helpers;
using ChatService.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ChatService.RabbitMQ
{
    public class MessageHandler
    {
        public static async Task SendMessage(string msgData)
        {
            Console.WriteLine($"[RabbitMQ]: {msgData}");

            var message = JsonConvert.DeserializeObject<Message>(msgData);
           
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();

            var options = optionsBuilder
                    .UseNpgsql("Host=localhost;Port=5432;Database=EDH_Chat_db;Username=postgres;Password=admin")
                    .Options;

            using (var context = new DataContext(options))
            {
                await context.Messages.AddAsync(message);
                await context.SaveChangesAsync();
            }
        }
    }
}
