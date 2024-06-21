using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UserService.Database;

namespace UserService.Models
{
    public class ItemPortfolio : IEntity
    {
        public ItemPortfolio(string name, string description, string stack, string repoLink, int userId)
        {
            Name = name;
            Description = description;
            Stack = stack;
            RepoLink = repoLink;

            UserId = userId;
        }

        public ItemPortfolio() { }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; } 

        public string Stack { get; set; }

        public string RepoLink { get; set; }

        public int UserId { get; set; }

        [JsonIgnore]
        [NotMapped]
        public User User { get; set; }
    }
}
