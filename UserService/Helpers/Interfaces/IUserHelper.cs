using System.Security.Claims;
using UserService.Models;
using UserService.Models.Enums;
using UserService.Models.Requests;

namespace UserService.Helpers.Interfaces
{
    public interface IUserHelper
    {
        Task<User> CreateUserAsync(RegisterRequest req);

        Task<User?> GetUserAsync(string username);
        Task<User?> GetUserAsync(string username, string email);
        Task<User?> GetUserAsync(int id);

        Task<User?> GetUserNoTrackingAsync(int id);

        Task<User?> GetUserAsync(ClaimsPrincipal claims);

        Task UpdateUserAsync(User user);

        Task<List<User>> GetUsersAsync(UserType type, Specialization? spec = null, Experience? exp = null);
    }
}
