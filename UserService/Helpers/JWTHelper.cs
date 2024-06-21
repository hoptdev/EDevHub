using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UserService.Helpers.Interfaces;
using UserService.Models;

namespace UserService.Helpers
{
    public class JWTHelper : IJWTHelper
    {
        public JwtSecurityToken CreateToken(User user)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Login), new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) };

            var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromHours(24)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return jwt;
        }

        public string GetToken(User user)
        {
            return new JwtSecurityTokenHandler().WriteToken(CreateToken(user));
        }
    }
}
