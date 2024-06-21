using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Helpers.Attributes;
using UserService.Models;
using UserService.Helpers.Interfaces;
using UserService.Models.Enums;
using UserService.Models.Requests;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private IUserHelper UserHelper { get; set; }

        private IEmailHelper EmailHelper { get; set; }


        public UserController(IUserHelper userHelper, IEmailHelper emailHelper)
        {
            UserHelper = userHelper;
            EmailHelper = emailHelper;
        }

        [HttpGet("getUsers")]
        [Authorize]
        [SwaggerHeader("Authorization", null)]
        public async Task<ActionResult<List<User>>> GetUsers(UserType type, Specialization? spec = null, Experience? exp = null)
        {
            var users = await UserHelper.GetUsersAsync(type, spec, exp);

            return Ok(users);
        }

        [HttpGet("getUser")]
        [Authorize]
        [SwaggerHeader("Authorization", null)]
        public async Task<ActionResult<User>> GetUser()
        {
            var user = await UserHelper.GetUserAsync(User);

            if (user == null) return BadRequest("Token invalid");

            return Ok(user);
        }

        [HttpGet("getUserById")]
        [Authorize]
        [SwaggerHeader("Authorization", null)]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await UserHelper.GetUserAsync(id);

            if (user == null) return BadRequest("Token invalid");

            return Ok(user);
        }

        [HttpPut("updateUser")]
        [Authorize]
        [SwaggerHeader("Authorization", null)]
        public async Task<ActionResult<User>> GetUser([FromForm] User user)
        {
            var mainUser = await UserHelper.GetUserNoTrackingAsync(user.Id);

            if (user is null) return BadRequest(null);

            if (user.Avatar is null)
            {
                user.Avatar = mainUser.Avatar;
                user.HashPassword = mainUser.HashPassword;
                user.Login = mainUser.Login;
            }

            await UserHelper.UpdateUserAsync(user);
                
            return Ok(user);
        }

        [HttpPut("uploadAvatar")]
        [Authorize]
        [SwaggerHeader("Authorization", null)]
        public async Task<IActionResult> UploadAvatar([FromForm] FileUploadRequest fileReq)
        {
            if (fileReq is null) return BadRequest();
            var file = fileReq.File;

            if (file is null) return BadRequest();

            var data = new byte[file.Length];

            using (var bstream = file.OpenReadStream())
            {
                await bstream.ReadAsync(data);
            }

            var user = await UserHelper.GetUserAsync(User);

            string path = $"avatars\\{user.Id}.{file.FileName}";

            if (!string.IsNullOrEmpty(user.Avatar) && user.Avatar != "avatars\\user.png")
                System.IO.File.Delete($"wwwroot\\{user.Avatar}");

            await System.IO.File.WriteAllBytesAsync($"wwwroot\\{path}", data);
            user.Avatar = path;

            await UserHelper.UpdateUserAsync(user);

            return Ok();
        }

        [HttpPut("uploadResume")]
        [Authorize]
        [SwaggerHeader("Authorization", null)]
        public async Task<IActionResult> UploadResume([FromForm] FileUploadRequest fileReq)
        {
            if (fileReq is null) return BadRequest();
            var file = fileReq.File;

            if (file is null) return BadRequest();

            var data = new byte[file.Length];

            using (var bstream = file.OpenReadStream())
            {
                await bstream.ReadAsync(data);
            }

            var user = await UserHelper.GetUserAsync(User);

            string path = $"resumes\\{user.Id}.{file.FileName}";

            if (!string.IsNullOrEmpty(user.Resume) && user.Resume != "resumes\\user.png")
                System.IO.File.Delete($"wwwroot\\{user.Resume}");

            await System.IO.File.WriteAllBytesAsync($"wwwroot\\{path}", data);
            user.Resume = path;

            await UserHelper.UpdateUserAsync(user);

            return Ok();
        }
    }
}
