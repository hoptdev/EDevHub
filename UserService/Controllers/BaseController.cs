using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Database;
using UserService.Helpers.Attributes;

namespace UserService.Controllers
{
    public class BaseController<Entity> : ControllerBase where Entity : class, IEntity
    {
        private readonly IDbRepository _repository;

        public BaseController(IDbRepository dbRepository)
        {
            _repository = dbRepository;
        }

        public virtual async Task<Entity> GetByIdAsync(int id)
        {
            var entity = await _repository.Get<Entity>(entiy => entiy.Id == id).FirstOrDefaultAsync();
            return entity;
        }

        public virtual async Task<IActionResult> UpdateAsync(Entity entity)
        {
            try
            {
                await _repository.Update(entity);
                await _repository.SaveChangesAsync();

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        public virtual async Task<IActionResult> DeleteById(int id)
        {
            var entity = await GetByIdAsync(id);

            await _repository.Remove(entity);

            return Ok();
        }

        public virtual async Task<IActionResult> Add(Entity entity)
        {
            var result = await _repository.Add(entity);
            await _repository.SaveChangesAsync();
            return Ok(result);
        }
    }
}
