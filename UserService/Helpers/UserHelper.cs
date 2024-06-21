using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UserService.Database;
using UserService.Models;
using UserService.Models.Requests;
using UserService.Helpers.Interfaces;
using UserService.Models.Enums;

namespace UserService.Helpers
{
    public class UserHelper : Helper<User>, IUserHelper
    {
        public UserHelper(IDbRepository dbRepository) : base(dbRepository)
        { }

        public async Task<User?> GetUserAsync(string username)
            => await dbRepository.Get<User>(x => x.Login == username || x.Email == username).FirstOrDefaultAsync();

        public async Task<User?> GetUserAsync(int id)
            => await dbRepository.Get<User>(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<User?> GetUserNoTrackingAsync(int id)
            => await dbRepository.Get<User>(x => x.Id == id).AsNoTracking().FirstOrDefaultAsync();

        public async Task<User?> GetUserAsync(string username, string email)
            => await dbRepository.Get<User>(x => x.Login == username || x.Email == email).FirstOrDefaultAsync();

        public async Task<List<User>> GetUsersAsync(UserType type, Specialization? spec = null, Experience? exp = null)
        {
            var usersReq = dbRepository.Get<User>(x => x.UserType == type);

            if (spec is not null) usersReq = usersReq.Where(x => x.Specialization == spec);
            if (exp is not null) usersReq = usersReq.Where(x => x.Experience == exp);

            return await usersReq.ToListAsync();
         }

        public async Task<User?> GetUserAsync(ClaimsPrincipal claims)
        {
            var login = claims.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (login is null) return null;
             
            return await GetByIdAsync(Convert.ToInt32(login));
        }

        public async Task<User> CreateUserAsync(RegisterRequest req)
        {
            User user = new User(req.Login, AuthHelper.Hash(req.Password), req.Email, req.userType);

            await dbRepository.Add(user);
            await dbRepository.SaveChangesAsync();

            return user;
        }

        public async Task UpdateUserAsync(User user)
            => await UpdateAsync(user);
    }
}
