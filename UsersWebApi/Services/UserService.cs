using UsersWebApi.Exceptions;
using UsersWebApi.Extensions;
using UsersWebApi.Interfaces;
using UsersWebApi.Requests;
using JwtConfiguration.Interfaces;
using JwtConfiguration.Models;

namespace UsersWebApi.Services
{
    public class UserService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher) : IUserService
    {
        public async Task<User> GetById(int id)
        {
            var user = await userRepository.GetById(id)
                ?? throw new UserNotFoundException();
            return user.MapToUser();
        }

        public async Task Update(UpdateUserRequest userRequest)
        {
            if (userRequest.Password != null)
                passwordHasher.Generate(userRequest.Password);

            await userRepository.Update(userRequest.MapToDBUser());
        }

        public async Task DeletebyId(int id)
        {
            await userRepository.DeleteById(id);
        }
    }
}
