using UsersWebApi.Models;
using UsersWebApi.Filters;
using Microsoft.OpenApi.Models;
using UsersWebApi.Extensions;
using UsersWebApi.Interfaces;
using UsersWebApi.Repositories;
using JwtConfiguration.Extensions;

namespace UsersWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddEndpointsApiExplorer()
                            .AddSwaggerGen(options =>
                            {
                                options.SwaggerDoc("Users", new OpenApiInfo
                                {
                                    Version = "v1",
                                    Title = "Пользователи",
                                    Description = $"Лр№4 Годов, Поршнев"
                                });
                            });

            builder.Services.Configure<MongoDBSettings>(
                builder.Configuration.GetRequiredSection("UsersDB"));

            builder.Services.AddJWTAuth();

            builder.Services.AddSingleton<IUserRepository, UserRepository>();
            builder.Services.AddServices();

            builder.Services.AddControllers(options => options.Filters.Add<ExceptionFilter>());

            var app = builder.Build();

            app.UseCors(cors =>
            {
                cors.AllowAnyHeader();
                cors.AllowAnyMethod();
                cors.AllowAnyOrigin();
            });

            app.UseCookiePolicy(new CookiePolicyOptions
            {
                Secure = CookieSecurePolicy.Always
            });

            app.UseSwagger()
               .UseSwaggerUI(options =>
               {
                   options.SwaggerEndpoint("/swagger/Users/swagger.json", "Users WebApi");
                   options.RoutePrefix = string.Empty;
               });

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}
