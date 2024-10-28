using UsersWebApi.Requests;
using UsersWebApi.Models;

namespace UsersWebApi.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> Register(RegisterUserRequest request, HttpContext context);

        Task<AuthResponse> Login(LoginUserRequest request, HttpContext context);

        Task<AuthResponse> RefreshToken(string refreshToken, HttpContext context);
    }
}
