using System.IdentityModel.Tokens.Jwt;
using UserService.Models;

namespace UserService.Helpers.Interfaces
{
    public interface IJWTHelper
    {
        public JwtSecurityToken CreateToken(User user);

        public string GetToken(User user);
    }
}
