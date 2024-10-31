using Ocelot.Middleware;
using Ocelot.DependencyInjection;
using JwtConfiguration.Extensions;

namespace Gateway
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("ocelot.json", false, true);
            builder.Services.AddOcelot(builder.Configuration);
            
            builder.Services.AddJWTAuth();
            
            var app = builder.Build();

            app.UseAuthentication();
            app.UseAuthorization();

            await app.UseOcelot();

            app.Run();
        }
    }
}
