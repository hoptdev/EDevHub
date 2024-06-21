using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using UserService.Models;

namespace UserService.Database
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> dbContextOptions) : base(dbContextOptions) { }

        public DbSet<User> Users { get; set; }

        public DbSet<ItemPortfolio> Portfolios { get; set; }

        public DbSet<Vacancy> Vacancies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
