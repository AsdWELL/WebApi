using Microsoft.Extensions.Caching.Distributed;
using SportNewsWebApi.Interfaces;
using SportNewsWebApi.Models;
using SportNewsWebApi.Requests;
using System.Text.Json;

namespace SportNewsWebApi.Services
{
    /// <summary>
    /// Сервис для работы с <see cref="SportNews"/>
    /// </summary>
    public class SportNewsService(
        ISportNewsRepository repository,
        IDistributedCache cache,
        ProducerService producerService) : ISportNewsService
    {
        public async Task<IEnumerable<SportNews>> GetAll()
        {
            return await repository.GetAll();
        }

        public async Task<SportNews?> GetById(int id)
        {
            SportNews? sportNews;

            string? sportNewsString = await cache.GetStringAsync(id.ToString());

            if (sportNewsString == null)
            {
                sportNews = await repository.GetById(id);

                if (sportNews != null)
                {
                    await cache.SetStringAsync(id.ToString(), JsonSerializer.Serialize(sportNews),
                        new DistributedCacheEntryOptions
                        {
                            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                        });
                }
            }
            else
                sportNews = JsonSerializer.Deserialize<SportNews>(sportNewsString);

            return sportNews;
        }

        public async Task<SportNews?> GetByIdWithoutCache(int id)
        {
            return await repository.GetById(id);
        }

        public async Task<int> Add(CreateSportNewsRequest request, CancellationToken stoppingToken)
        {
            int id = await repository.Add(new SportNews
            {
                Title = request.Title,
                Content = request.Content
            });

            await producerService.ProduceAsync(stoppingToken,
                new ProducerMessage
                {
                    ObjectId = id,
                    UserId = request.UserId
                });

            return id;
        }

        public async Task<bool> Delete(int id)
        {
            await cache.RemoveAsync(id.ToString());
            
            return await repository.Delete(id);
        }

        public async Task<bool> Update(SportNews sportNews)
        {
            await cache.RemoveAsync(sportNews.Id.ToString());

            return await repository.Update(sportNews);
        }
    }
}
