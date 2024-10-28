using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UsersWebApi.Controllers
{
    [ApiController]
    [Authorize]
    public class AuthorizedUserController : ControllerBase
    {
        public int UserId => Convert.ToInt32(
            User.Claims.ToList().First(claim =>
                claim.Type.Equals("userId")).Value);
    }
}
