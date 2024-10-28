using SportNewsWebApi.Filters;
using SportNewsWebApi.Interfaces;
using SportNewsWebApi.Models;
using SportNewsWebApi.Repositories;
using SportNewsWebApi.Services;
using Microsoft.OpenApi.Models;

namespace SportNewsWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers(options => options.Filters.Add<ExceptionFilter>());
            
            builder.Services.AddEndpointsApiExplorer()
                            .AddSwaggerGen(options =>
                            {
                                options.SwaggerDoc("SportNews", new OpenApiInfo
                                {
                                    Version = "v1",
                                    Title = "Лента спортивных новостей",
                                    Description = $"Лр№4 Годов, Поршнев"
                                });
                            });

            builder.Services.AddDistributedMemoryCache();
            builder.Services.Configure<MongoDBSettings>(
                builder.Configuration.GetRequiredSection("SportNewsDB"));
            builder.Services.AddSingleton<ISportNewsRepository, SportNewsMongoDBRepository>()
                .AddScoped<ISportNewsService, SportNewsService>()
                .AddSingleton<ProducerService>()
                .AddHostedService<ConsumerService>();

            var app = builder.Build();

            app.UseCors(cors =>
            {
                cors.AllowAnyHeader();
                cors.AllowAnyMethod();
                cors.AllowAnyOrigin();
            });

            app.UseSwagger()
               .UseSwaggerUI(options =>
               {
                   options.SwaggerEndpoint("/swagger/SportNews/swagger.json", "SportNews WebApi");
                   options.RoutePrefix = string.Empty;
               });

            app.MapControllers();
            app.Run();
        }
    }
}
