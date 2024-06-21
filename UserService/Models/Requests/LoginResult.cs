namespace UserService.Models.Requests
{
    public class LoginResult
    {
        public LoginResult(string? status, int userId, string token)
        {
            Status = status;
            UserId = userId;
            Token = token;
        }

        public string? Status { get; set; }

        public int UserId { get; set; }

        public string Token { get; set; }
    }
}
