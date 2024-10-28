using System.ComponentModel.DataAnnotations;

namespace UsersWebApi.Requests
{
    public class RegisterUserRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
