using JwtConfiguration.Models;

namespace JwtConfiguration.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User? user = null);
    }
}
