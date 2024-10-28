using UsersWebApi.Models;
using UsersWebApi.Filters;
using Microsoft.OpenApi.Models;
using UsersWebApi.Extensions;
using UsersWebApi.Interfaces;
using UsersWebApi.Repositories;

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
                builder.Configuration.GetRequiredSection("SportNewsDB"));

            var authSection = builder.Configuration.GetRequiredSection(nameof(TokenOptions));

            builder.Services.Configure<TokenOptions>(authSection);
            builder.Services.AddJWTAuth(authSection.Get<TokenOptions>());

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
                   options.SwaggerEndpoint("/swagger/Users/swagger.json", "SportNews WebApi");
                   options.RoutePrefix = string.Empty;
               });

            app.MapControllers();
            app.Run();
        }
    }
}
