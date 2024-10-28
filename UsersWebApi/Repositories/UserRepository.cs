using Microsoft.Extensions.Options;
using MongoDB.Driver;
using UsersWebApi.Interfaces;
using UsersWebApi.Models;

namespace UsersWebApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<DBUser> _usersCollection;

        public UserRepository(IOptions<MongoDBSettings> options)
        {
            var mongoClient = new MongoClient(options.Value.ConnectionString);

            var db = mongoClient.GetDatabase(options.Value.DatabaseName);

            _usersCollection = db.GetCollection<DBUser>(options.Value.CollectionName);
        }

        public async Task<DBUser?> GetById(int id)
        {
            return await (await _usersCollection.FindAsync(user => user.Id == id)).FirstOrDefaultAsync();
        }

        public async Task<DBUser?> GetByLogin(string login)
        {
            return await (await _usersCollection.FindAsync(user => user.Login == login)).FirstOrDefaultAsync();
        }

        public async Task<DBUser?> GetByRefreshToken(string refreshToken)
        {
            return await (await _usersCollection.FindAsync(user => user.RefreshToken == refreshToken)).FirstOrDefaultAsync();
        }

        public async Task<int> AddUser(DBUser user)
        {
            user.Id = new Random().Next(0, (int)Math.Pow(10, 6));

            await _usersCollection.InsertOneAsync(user);

            return user.Id;
        }

        public async Task DeleteById(int id)
        {
            await _usersCollection.DeleteOneAsync(user => user.Id == id);
        }

        public async Task DeleteByLogin(string login)
        {
            await _usersCollection.DeleteOneAsync(user => user.Login == login);
        }

        public async Task Update(DBUser user)
        {
            var filter = Builders<DBUser>.Filter.Eq(x => x.Id, user.Id);

            var updatesBuilder = Builders<DBUser>.Update;
            var updates = new List<UpdateDefinition<DBUser>>
            {
                updatesBuilder.Set(x => x.HashedPassword, user.HashedPassword)
            };

            if (user.Name != null)
                updates.Add(updatesBuilder.Set(x => x.Name, user.Name));

            if (user.Surname != null)
                updates.Add(updatesBuilder.Set(x => x.Surname, user.Surname));

            if (user.Login != null)
                updates.Add(updatesBuilder.Set(x => x.Login, user.Login));

            await _usersCollection.UpdateOneAsync(filter, updatesBuilder.Combine(updates));
        }

        public async Task UpdateRefreshToken(int id, string refreshToken, DateTime refreshTokenExpiredAt)
        {
            var filter = Builders<DBUser>.Filter.Eq(user => user.Id, id);
            var update = Builders<DBUser>.Update
                .Set(user => user.RefreshToken, refreshToken)
                .Set(user => user.RefreshTokenExpiredAt, refreshTokenExpiredAt);
            
            await _usersCollection.UpdateOneAsync(filter, update);
        }

        public async Task<bool> AddRegisteredObject(int id)
        {
            var filter = Builders<DBUser>.Filter.Eq(user => user.Id, id);
            var update = Builders<DBUser>.Update
                .Inc(user => user.RegisteredObjects, 1);

            return (await _usersCollection.UpdateOneAsync(filter, update)).ModifiedCount > 0;
        }
    }
}