using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.Helpers.Attributes;
using UserService.Helpers.Interfaces;
using UserService.Models.Enums;
using UserService.Models;
using UserService.Helpers;
using UserService.Database;
using UserService.Models.Requests;
using Microsoft.AspNet.SignalR;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    public class VacancyController : ControllerBase
    {
        private VacancyHelper Helper { get; set; }

        private IUserHelper UserHelper { get; set; }  

        public VacancyController(IHelper<Vacancy> helper, IUserHelper uHelper)
        {
            Helper = (VacancyHelper)helper;
            UserHelper = uHelper;
        }

        [HttpGet("getAll")]
        [Authorize]
        public async Task<ActionResult<List<Vacancy>>> GetAll(Experience? exp)
        {
            var result = exp is null ? await Helper.GetAll() : await Helper.GetByExp(exp);

            if (result is null) return BadRequest(new List<Vacancy>() { });

            result.Reverse();

            return Ok(result);
        }

        [HttpGet("getByUserId")]
        [Authorize]
        public async Task<ActionResult<Vacancy>> GetByUserId(int id)
        {
            var result = await Helper.GetByUserId(id);

            if (result is null) return BadRequest("Result is null");

            return Ok(result);
        }

        [HttpPost("create")]
        [Authorize]
        [SwaggerHeader("Authorization", null)]
        public async Task<ActionResult<int>> Create([FromForm] VacancyRequest req)
        {
            var user = await UserHelper.GetUserAsync(this.User);

            if (user is null) return BadRequest("user not found");

            var result = await Helper.Add(new Vacancy(req.Name, req.Pay, req.Description, user.Id, req.Experience));

            return Ok(result);
        }

        [HttpPost("update")]
        [Authorize]
        [SwaggerHeader("Authorization", null)]
        public async Task<IActionResult> Update([FromForm] Vacancy vacancy)
        {
            await Helper.UpdateAsync(vacancy);

            return Ok();
        }

        [HttpDelete("removeById")]
        [Authorize]
        [SwaggerHeader("Authorization", null)]
        public async Task<IActionResult> Remove(int vacancyId)
        {
            var user = await UserHelper.GetUserAsync(this.User);

            var vacancy = (await Helper.GetByUserId(user.Id)).FirstOrDefault(x => x.Id == vacancyId);
            if (vacancy == null) return BadRequest();

            await Helper.DeleteById(vacancyId);

            return Ok();
        }
    }
}
