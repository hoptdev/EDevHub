using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using UserService.Database;
using UserService.Models.Enums;

namespace UserService.Models
{
    public class User : IEntity
    {
        public User(string login, string hash, string email, UserType user)
        {
            Login = login;
            HashPassword = hash;
            Email = email;

            UserType = user;
            Username = login;

            Avatar = @"avatars\user.png";
        }

        public User() { }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        public string? Description { get; set; }

        public string? Avatar { get; set; }

        public string? Resume { get; set; }

        public string? Phone { get; set; }

        [Required]
        public UserType UserType { get; set; }

        public Specialization? Specialization { get; set; }

        public Experience? Experience { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [Newtonsoft.Json.JsonIgnore]
        public string Login { get; set; }

        [Required]
        [Newtonsoft.Json.JsonIgnore]
        public string HashPassword { get; set; }
    }
}
