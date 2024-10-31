using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.Annotations;

namespace UsersWebApi.Requests
{
    public class UpdateUserRequest
    {
        [BindNever]
        [SwaggerIgnore]
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Surname { get; set; }

        public string? Login { get; set; }

        public string? Password { get; set; }
    }
}
