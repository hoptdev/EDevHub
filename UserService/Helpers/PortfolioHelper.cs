using Microsoft.EntityFrameworkCore;
using UserService.Database;
using UserService.Models;

namespace UserService.Helpers
{
    public class PortfolioHelper : Helper<ItemPortfolio>
    {
        public PortfolioHelper(IDbRepository dbRepository) : base(dbRepository)
        {
        }

        public async Task<List<ItemPortfolio>> GetByUserId(int userId)
        {
            return await dbRepository.Get<ItemPortfolio>(x => x.UserId == userId).ToListAsync();
        }
    }
}
