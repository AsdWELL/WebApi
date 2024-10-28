using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UsersWebApi.Interfaces;
using UsersWebApi.Models;

namespace UsersWebApi.Services
{
    public class TokenService(IOptions<TokenOptions> options, IUserRepository userRepository) : ITokenService
    {
        private readonly TokenOptions _tokenOptions = options.Value;

        private List<Claim> CreateClaims(User? user)
        {
            return user == null ? [] : [new Claim("userId", user.Id.ToString())];
        }

        public string GenerateToken(User? user = null)
        {
            var claims = CreateClaims(user);

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.Key)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                signingCredentials: signingCredentials,
                issuer: _tokenOptions.Issuer,
                audience: _tokenOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_tokenOptions.ExpiresAfterHours));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
