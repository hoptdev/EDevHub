using Microsoft.EntityFrameworkCore;
using UserService.Database;
using UserService.Helpers.Interfaces;
using UserService.Models;
using UserService.Models.Enums;

namespace UserService.Helpers
{
    public class VacancyHelper : Helper<Vacancy>
    {
        public VacancyHelper(IDbRepository dbRepository) : base(dbRepository)
        { }

        public async Task<List<Vacancy>> GetByUserId(int userId)
        {
            return await dbRepository.Get<Vacancy>(x => x.UserId == userId).ToListAsync();
        }

        public async Task<List<Vacancy>> GetByExp(Experience? experience)
        {
            return await dbRepository.Get<Vacancy>(x => x.Experience == experience).ToListAsync();
        }

        public override async Task<List<Vacancy>> GetAll()
        {
            var entity = dbRepository.Get<Vacancy>().Include(x=>x.User);
            if (entity is not null) return await entity.ToListAsync();

            return new List<Vacancy>() { };
        }
    }
}
