using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using UserService.Database;
using UserService.Models.Enums;

namespace UserService.Models
{
    public class Vacancy : IEntity
    {
        [Key]
        public int Id { get; set; }
         
        public string Name { get; set; }    

        public decimal Pay { get; set; }

        public string Description { get; set; }

        public Experience Experience { get; set; } 

        public int UserId { get; set; }

        public User? User { get; set; }

        public Vacancy() { }

        public Vacancy(string name, decimal pay, string description, int userId, Experience exp)
        {
            Name = name;
            Pay = pay;
            Description = description;
            UserId = userId;

            Experience = exp;
        }
    }
}
