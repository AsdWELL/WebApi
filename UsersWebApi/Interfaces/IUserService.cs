using UsersWebApi.Models;
using UsersWebApi.Requests;

namespace UsersWebApi.Interfaces
{
    public interface IUserService
    {
        Task<User> GetById(int id);

        Task Update(UpdateUserRequest userRequest);

        Task DeletebyId(int id);
    }
}
