using UsersWebApi.Models;

namespace UsersWebApi.Interfaces
{
    public interface IUserRepository
    {
        Task<int> AddUser(DBUser user);

        Task<DBUser?> GetByLogin(string login);

        Task<DBUser?> GetById(int id);

        Task<DBUser?> GetByRefreshToken(string refreshToken);

        Task UpdateRefreshToken(int id, string refreshToken, DateTime refreshTokenExpiredAt);

        Task Update(DBUser user);

        Task DeleteByLogin(string login);

        Task DeleteById(int id);

        Task<bool> AddRegisteredObject(int id);
    }
}
