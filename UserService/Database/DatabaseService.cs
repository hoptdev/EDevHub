using Microsoft.EntityFrameworkCore;

namespace UserService.Database
{
    public class DatabaseService
    {
        public static void ConfigureUserService(IServiceCollection services)
        {
            string mysql = "Host=localhost;Port=5432;Database=EDH_db;Username=postgres;Password=admin";
            services.AddDbContext<DataContext>(options => options.UseNpgsql(mysql));

            services.AddScoped<IDbRepository, DbRepository>();
        }
    }
}
