using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using UserService.Database;
using UserService.Helpers;
using UserService.Helpers.Attributes;
using UserService.Helpers.Interfaces;
using UserService.Models;

namespace UserService
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            DatabaseService.ConfigureUserService(services);
            services.AddScoped<IUserHelper, UserHelper>();
            services.AddScoped<IEmailHelper, EmailHelper>();
            services.AddScoped<IJWTHelper, JWTHelper>();
            services.AddScoped(typeof(IHelper<>), typeof(Helper<>));

            services.AddScoped<IHelper<ItemPortfolio>, PortfolioHelper>();
            services.AddScoped<IHelper<Vacancy>, VacancyHelper>();

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

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "You api title", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
      {
        {
          new OpenApiSecurityScheme
          {
            Reference = new OpenApiReference
              {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
              },
              Scheme = "oauth2",
              Name = "Bearer",
              In = ParameterLocation.Header,

            },
            new List<string>()
          }
        });
            });
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
                endpoints.MapControllers();
            });
        }
    }
}
