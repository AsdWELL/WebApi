using System.ComponentModel.DataAnnotations;

namespace UsersWebApi.Requests
{
    public class LoginUserRequest
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
