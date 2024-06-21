using Microsoft.AspNetCore.Mvc;
using UserService.Helpers;
using UserService.Models.Requests;
using UserService.Models;
using UserService.Helpers.Interfaces;
using Microsoft.AspNetCore.Authorization;
using UserService.Helpers.Attributes;

namespace UserService.Controllers
{
    [Route("api/[controller]")]
    public class PortfolioController : ControllerBase
    {
        private PortfolioHelper Helper { get; set; }

        private IUserHelper UserHelper { get; set; }

        public PortfolioController(IHelper<ItemPortfolio> helper, IUserHelper uHelper)
        {
            Helper = (PortfolioHelper)helper;
            UserHelper = uHelper;
        }

        [HttpGet("getByUserId")]
        public async Task<ActionResult<List<ItemPortfolio>>> GetByUserId(int id)
        {
            var result = await Helper.GetByUserId(id);

            return Ok(result);
        }

        [HttpPost("create")]
        [Authorize]
        [SwaggerHeader("Authorization", null)]
        public async Task<ActionResult<int>> Create([FromForm] ItemPortfolioReq req)
        {
            var user = await UserHelper.GetUserAsync(this.User);

            if (user is null) return BadRequest(-1);

            var result = await Helper.Add(new ItemPortfolio(req.Name, req.Description, req.Stack, req.RepoLink, user.Id));

            return Ok(result);
        }

        [HttpPost("update")]
        [Authorize]
        [SwaggerHeader("Authorization", null)]
        public async Task<IActionResult> Update([FromForm] ItemPortfolio vacancy)
        {
            await Helper.UpdateAsync(vacancy);

            return Ok();
        }

        [HttpDelete("removeById")]
        [Authorize]
        [SwaggerHeader("Authorization", null)]
        public async Task<IActionResult> Remove(int portfolioId)
        {
            var user = await UserHelper.GetUserAsync(this.User);

            var vacancy = (await Helper.GetByUserId(user.Id)).FirstOrDefault(x => x.Id == portfolioId);
            if (vacancy == null) return BadRequest();

            await Helper.DeleteById(portfolioId);

            return Ok();
        }
    }
}
