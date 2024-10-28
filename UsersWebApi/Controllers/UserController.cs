using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UsersWebApi.Interfaces;
using UsersWebApi.Requests;
using UsersWebApi.Models;

namespace UsersWebApi.Controllers
{
    [Route("api/[controller]")]
    public class UserController(IUserService userService) : AuthorizedUserController
    {
        [HttpGet]
        public async Task<IActionResult> GetCurrentUser()
        {
            return Ok(await userService.GetById(UserId));
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateUserRequest userRequest)
        {
            userRequest.Id = UserId;

            await userService.Update(userRequest);

            return Ok(new { Message = $"Информация обновлена" });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(IOptions<TokenOptions> tokenOptions)
        {
            await userService.DeletebyId(UserId);

            Response.Cookies.Delete(tokenOptions.Value.CookieTitle);

            return Ok(new { Message = "Пользователь удален" });
        }
    }
}
