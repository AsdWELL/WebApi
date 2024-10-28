using UsersWebApi.Models;
using UsersWebApi.Requests;

namespace UsersWebApi.Extensions
{
    public static class Mapper
    {
        public static User MapToUser(this DBUser user)
        {
            return new User
            {
                Id = user.Id,
                Login = user.Login,
                Name = user.Name,
                Surname = user.Surname,
                RegisteredObjects = user.RegisteredObjects,
            };
        }

        public static DBUser MapToDBUser(this UpdateUserRequest userRequest)
        {
            return new DBUser
            {
                Id = userRequest.Id,
                Name = userRequest.Name,
                Surname = userRequest.Surname,
                Login = userRequest.Login,
                HashedPassword = userRequest.Password
            };
        }
    }
}
