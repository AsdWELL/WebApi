using SportNewsWebApi.Repositories;
using SportNewsWebApi.Models;
using SportNewsWebApi.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SportNewsWebApi.Requests;

namespace Tests
{
    public class SportNewsTests
    {
        private readonly SportNewsService _service;

        public SportNewsTests()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var options = configuration.GetRequiredSection("SportNewsDB").Get<MongoDBSettings>();

            var repository = new SportNewsMongoDBRepository(Options.Create(options));

            _service = new SportNewsService(repository,
                new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions())),
                new ProducerService());
        }

        [Fact]
        public async Task DeleteAll()
        {
            var sportNews = await _service.GetAll();

            foreach (var s in sportNews)
                await _service.Delete(s.Id);

            Assert.Empty(await _service.GetAll());
        }

        [Fact]
        public async Task GetSportNewsWithoutCache()
        {
            for (int i = 1; i <= 10000; i++)
                _ = await _service.GetByIdWithoutCache(1);
        }

        [Fact]
        public async Task GetSportsNews()
        {
            for (int i = 1; i <= 10000; i++)
                _ = await _service.GetById(1);
        }

        [Theory]
        [InlineData(100)]
        [InlineData(10000)]
        public async Task Add(int count)
        {
            CreateSportNewsRequest request = new CreateSportNewsRequest
            {
                Title = "Заголовок",
                Content = "Контент",
                UserId = 485805
            };

            int records = (await _service.GetAll()).ToList().Count;

            for (int i = 0; i < count; i++)
                await _service.Add(request, CancellationToken.None);

            Assert.Equal(count, (await _service.GetAll()).ToList().Count - records);
        }
    }
}
