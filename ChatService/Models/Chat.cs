using ChatService.Database;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UserService.Models;

namespace ChatService.Models
{
    public class Chat : IEntity
    {
        public Chat(int user1Id, int user2Id)
        {
            User1Id = user1Id;
            User2Id = user2Id;
        }

        public Chat() { }

        [Key]
        public int Id { get; set; }

        public int User1Id { get; set; }
        [NotMapped]
        public User User1 { get; set; }

        public int User2Id { get; set; }
        [NotMapped]
        public User User2 { get; set; }
    }
}
