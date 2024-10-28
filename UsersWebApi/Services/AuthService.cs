using Microsoft.Extensions.Options;
using UsersWebApi.Exceptions;
using UsersWebApi.Extensions;
using UsersWebApi.Interfaces;
using UsersWebApi.Models;
using UsersWebApi.Requests;

namespace UsersWebApi.Services
{
    public class AuthService(
        IPasswordHasher passwordHasher,
        IUserRepository userRepository,
        ITokenService tokenService,
        IOptions<TokenOptions> tokenOptions) : IAuthService
    {
        private TokenOptions TokenOptionsValue => tokenOptions.Value;

        private void SetTokenInCookie(string token, HttpContext context)
        {
            context.Response.Cookies.Append(TokenOptionsValue.CookieTitle, token,
                new CookieOptions { Secure = true, HttpOnly = false });
        }

        public async Task<AuthResponse> Register(RegisterUserRequest userRequest, HttpContext context)
        {
            if ((await userRepository.GetByLogin(userRequest.Login)) != null)
                throw new LoginAlreadyTakenException(userRequest.Login);

            string hashedPassword = passwordHasher.Generate(userRequest.Password);

            string refreshToken = tokenService.GenerateToken();

            DBUser user = new DBUser
            {
                Name = userRequest.Name,
                Surname = userRequest.Surname,
                Login = userRequest.Login,
                HashedPassword = hashedPassword,
                RefreshToken = refreshToken,
                RefreshTokenExpiredAt = DateTime.UtcNow.AddHours(TokenOptionsValue.ExpiresAfterHours),
                RegisteredObjects = 0
            };

            user.Id = await userRepository.AddUser(user);

            string accessToken = tokenService.GenerateToken(user.MapToUser());

            SetTokenInCookie(accessToken, context);

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<AuthResponse> Login(LoginUserRequest userRequest, HttpContext context)
        {
            DBUser user = await userRepository.GetByLogin(userRequest.Login)
                ?? throw new LoginNotFoundException(userRequest.Login);

            if (!passwordHasher.Verify(userRequest.Password, user.HashedPassword))
                throw new WrongPasswordException();

            string accessToken = tokenService.GenerateToken(user.MapToUser());

            SetTokenInCookie(accessToken, context);

            return new AuthResponse
            {
                AccessToken = accessToken,
                RefreshToken = user.RefreshToken
            };
        }

        public async Task<AuthResponse> RefreshToken(string refreshToken, HttpContext context)
        {
            DBUser user = await userRepository.GetByRefreshToken(refreshToken)
                ?? throw new RefreshTokenNotFoundException();

            string newToken = tokenService.GenerateToken(user.MapToUser());
            SetTokenInCookie(newToken, context);

            string newRefreshToken = tokenService.GenerateToken();
            await userRepository.UpdateRefreshToken(user.Id,
                newRefreshToken,
                DateTime.UtcNow.AddHours(TokenOptionsValue.ExpiresAfterHours));

            return new AuthResponse
            {
                AccessToken = newToken,
                RefreshToken = newRefreshToken
            }; ;
        }
    }
}
