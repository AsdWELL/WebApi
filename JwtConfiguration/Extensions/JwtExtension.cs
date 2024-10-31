using Microsoft.Extensions.DependencyInjection;
using System.Text;
using JwtConfiguration.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace JwtConfiguration.Extensions
{
    public static class JwtExtension
    {
        public static IServiceCollection AddJWTAuth(this IServiceCollection services)
        {
            IConfiguration tokenConfig;
            
            using (var resourceStream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("JwtConfiguration.TokenOptions.json"))
            {
                tokenConfig = new ConfigurationBuilder()
                    .AddJsonStream(resourceStream)
                    .Build();
            }

            var tokenSection = tokenConfig.GetSection(nameof(TokenOptions));
            
            services.Configure<TokenOptions>(tokenSection);
            
            var tokenOptions = tokenSection.Get<TokenOptions>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.ClaimsIssuer = tokenOptions.Issuer;
                    options.SaveToken = true;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = tokenOptions.Issuer,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.Key))
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies[tokenOptions.CookieTitle];

                            return Task.CompletedTask;
                        },

                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception is SecurityTokenExpiredException)
                                context.Response.Headers.Append("Token-Expired", "true");

                            return Task.CompletedTask;
                        }
                    };
                });

            return services;
        }
    }
}
