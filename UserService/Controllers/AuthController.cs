using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Helpers.Attributes;
using System.Diagnostics;
using System.Net;
using System.Runtime.InteropServices;
using UserService.Models;
using UserService.Models.Requests;
using System.ComponentModel.DataAnnotations;
using UserService.Helpers.Interfaces;
using UserService.Helpers;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private IUserHelper UserHelper { get; set; }

        private IEmailHelper EmailHelper { get; set; }

        private IJWTHelper JwtHelper { get; set; }

        public AuthController(IUserHelper userHelper, IEmailHelper emailHelper, IJWTHelper jwtHelper)
        {
            UserHelper = userHelper;
            JwtHelper = jwtHelper;
            EmailHelper = emailHelper;
        }

        [HttpPost("signUp")]
        public async Task<ActionResult<int>> SignUp([FromForm] RegisterRequest req)
        {
            User? userMb = await UserHelper.GetUserAsync(req.Login, req.Email);

            if (EmailHelper.IsEmailValid(req.Email) && userMb == null)
            {
                var user = await UserHelper.CreateUserAsync(req);

                return Ok(new LoginResult("Success", user.Id, JwtHelper.GetToken(user)));
            }
            else
            {
                return BadRequest("Email or login exists.");
            }
        }

        [HttpPost("signIn")]
        public async Task<ActionResult<LoginResult>> SignIn([FromForm] LoginRequest req)
        {
            User? user = await UserHelper.GetUserAsync(req.Login, req.Email);

            if (user != null)
                if (AuthHelper.Verify(req.Password, user.HashPassword))
                    return Ok(new LoginResult("Success", user.Id, JwtHelper.GetToken(user)));


            return BadRequest(new LoginResult("Error", 0, string.Empty));
        }
    }
}
