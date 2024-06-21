using ChatService.Database;
using System.ComponentModel.DataAnnotations;

namespace ChatService.Models
{
    public class Message : IEntity
    {
        public Message() { }

        public Message(string text, int fromId, int chatId)
        {
            Text = text;
            FromId = fromId;
            ChatId = chatId;

            TimeSpan = TimeSpan.FromTicks(DateTime.Now.Ticks);
        }

        [Key]
        public int Id { get; set; }

        public string Text { get; set; }

        public int FromId { get; set; } 

        public int ChatId { get; set; }

        public TimeSpan TimeSpan { get; set; }
    }
}
