using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserService.Database;

namespace UserService.Helpers
{
    public class Helper<Entity> : IHelper<Entity> where Entity : class, IEntity
    {
        public readonly IDbRepository dbRepository;

        public Helper(IDbRepository dbRepository)
        {
            this.dbRepository = dbRepository;
        }

        public virtual async Task<Entity?> GetByIdAsync(int id)
        {
            var entity = await dbRepository.Get<Entity>(entiy => entiy.Id == id).FirstOrDefaultAsync();
            return entity;
        }

        public virtual async Task<List<Entity>> GetAll()
        {
            var entity = dbRepository.Get<Entity>();
            if (entity is not null) return await entity.ToListAsync();

            return new List<Entity>() { };
        }

        public virtual async Task UpdateAsync(Entity entity)
        {
            await dbRepository.Update(entity);
            await dbRepository.SaveChangesAsync();
        }

        public virtual async Task DeleteById(int id)
        {
            var entity = await GetByIdAsync(id);

            await dbRepository.Remove(entity);
            await dbRepository.SaveChangesAsync();
        }

        public virtual async Task<int> Add(Entity entity)
        {
            var result = await dbRepository.Add(entity);
            await dbRepository.SaveChangesAsync();

            return result;
        }
    }
}
