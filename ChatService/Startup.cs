using ChatService.Database;
using ChatService.Helpers;
using ChatService.Hubs;
using ChatService.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UserService;
using UserService.Helpers;
using UserService.Helpers.Attributes;
using UserService.Helpers.Interfaces;

namespace ChatService
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            string mysql = "Host=localhost;Port=5432;Database=EDH_Chat_db;Username=postgres;Password=admin";
            services.AddDbContext<DataContext>(options => options.UseNpgsql(mysql));
            services.AddScoped<IDbRepository, DbRepository>();

            string mainDb = "Host=localhost;Port=5432;Database=EDH_db;Username=postgres;Password=admin";
            services.AddDbContext<UserService.Database.DataContext>(options => options.UseNpgsql(mainDb));
            services.AddScoped<UserService.Database.IDbRepository, UserService.Database.DbRepository>();

            services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

            services.AddScoped<Helpers.IHelper<Message>, MessageHelper>();
            services.AddScoped<Helpers.IHelper<Chat>, ChatHelper>();
            services.AddScoped<IUserHelper, UserHelper>();

            services.AddAuthorization();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthOptions.ISSUER,
                        ValidateAudience = true,
                        ValidAudience = AuthOptions.AUDIENCE,
                        ValidateLifetime = true,
                        IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                    };
                });

            services.AddSwaggerGen(c => c.OperationFilter<SwaggerHeaderFilter>());

            services.AddControllers().AddNewtonsoftJson(options =>
             options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter()));

            services.AddCors(c =>
            {
                c.AddPolicy("GoodPolicy", policyBuilder => policyBuilder
                .SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

                c.DefaultPolicyName = "GoodPolicy";
            });

            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseSwagger()
               .UseSwaggerUI(c =>
               {
                   c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
               });

            app.UseDefaultFiles();
            app.UseStaticFiles();


            app.UseCors("GoodPolicy");

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(delegate (IEndpointRouteBuilder endpoints)
            {
                endpoints.MapHub<ChatHub>("/chat/hub");
                endpoints.MapControllers();
            });
        }
    }
}
