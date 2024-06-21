namespace UserService.Models.Requests
{
    public class LoginRequest
    {
        public string? Login { get; set; }

        public string? Email { get; set; }

        public string Password { get; set; }
    }
}
