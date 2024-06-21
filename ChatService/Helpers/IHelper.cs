using ChatService.Database;

namespace ChatService.Helpers
{
    public interface IHelper<T> where T : IEntity
    {
        public Task<T?> GetByIdAsync(int id);

        public Task<List<T>> GetAll();

        public Task UpdateAsync(T entity);

        public Task DeleteById(int id);

        public Task<int> Add(T entity);
    }
}
