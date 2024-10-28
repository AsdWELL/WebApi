using UsersWebApi.Models;

namespace UsersWebApi.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User? user = null);
    }
}
