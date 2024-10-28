using SportNewsWebApi.Interfaces;
using SportNewsWebApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ZstdSharp.Unsafe;

namespace SportNewsWebApi.Repositories
{
    /// <summary>
    /// Класс репозитория MongoDB для <see cref="SportNews"/>
    /// </summary>
    public class SportNewsMongoDBRepository : ISportNewsRepository
    {
        private readonly IMongoCollection<SportNews> _sportNewsCollection;

        public SportNewsMongoDBRepository(IOptions<MongoDBSettings> options)
        {
            var mongoClient = new MongoClient(options.Value.ConnectionString);

            var db = mongoClient.GetDatabase(options.Value.DatabaseName);

            _sportNewsCollection = db.GetCollection<SportNews>(options.Value.CollectionName);
        }

        ///<inheritdoc/>
        public async Task<IEnumerable<SportNews>> GetAll()
        {
            return await (await _sportNewsCollection.FindAsync(_ => true)).ToListAsync();
        }

        ///<inheritdoc/>
        public async Task<SportNews?> GetById(int id)
        {
            return await (await _sportNewsCollection.FindAsync(x => x.Id == id)).FirstOrDefaultAsync();
        }

        ///<inheritdoc/>
        public async Task<int> Add(SportNews sportNews)
        {
            sportNews.Id = new Random().Next(0, (int)Math.Pow(10, 6));

            await _sportNewsCollection.InsertOneAsync(sportNews);

            return sportNews.Id;
        }

        ///<inheritdoc/>
        public async Task<bool> Delete(int id)
        {
            return (await _sportNewsCollection.DeleteOneAsync(x => x.Id == id)).DeletedCount > 0;
        }

        ///<inheritdoc/>
        public async Task<bool> Update(SportNews sportNews)
        {
            return (await _sportNewsCollection.ReplaceOneAsync(x => x.Id == sportNews.Id, sportNews)).ModifiedCount > 0;
        }

        public async Task ConfirmSportNews(int id, int userId, DateTime confirmationTime)
        {
            var filter = Builders<SportNews>.Filter.Eq(news => news.Id, id);
            var update = Builders<SportNews>.Update
                .Set(news => news.UserId, userId)
                .Set(news => news.ConfirmationTime, confirmationTime);

            await _sportNewsCollection.UpdateOneAsync(filter, update);
        }
    }
}
