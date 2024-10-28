using Microsoft.AspNetCore.Mvc;
using UsersWebApi.Interfaces;
using UsersWebApi.Requests;

namespace UsersWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterUserRequest user)
        {
            return Ok(await authService.Register(user, HttpContext));
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginUserRequest user)
        {
            return Ok(await authService.Login(user, HttpContext));
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(string refreshToken)
        {
            return Ok(await authService.RefreshToken(refreshToken, HttpContext));
        }
    }
}
