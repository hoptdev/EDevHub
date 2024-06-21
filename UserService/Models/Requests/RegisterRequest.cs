using UserService.Models.Enums;

namespace UserService.Models.Requests
{
    public class RegisterRequest
    {
        public string Login { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public UserType userType { get; set; }
    }
}
