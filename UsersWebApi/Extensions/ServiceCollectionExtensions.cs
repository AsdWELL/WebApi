using JwtConfiguration.Interfaces;
using JwtConfiguration.Services;
using UsersWebApi.Interfaces;
using UsersWebApi.Services;

namespace UsersWebApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher, PasswordHasher>()
                .AddScoped<ITokenService, TokenService>()
                .AddScoped<IAuthService, AuthService>()
                .AddScoped<IUserService, UserService>()
                .AddSingleton<ProducerService>()
                .AddHostedService<ConsumerService>();

            return services;
        }
    }
}
