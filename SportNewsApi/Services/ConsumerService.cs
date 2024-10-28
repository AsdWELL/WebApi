using Confluent.Kafka;
using SportNewsWebApi.Interfaces;
using SportNewsWebApi.Json;
using SportNewsWebApi.Models;

namespace SportNewsWebApi.Services
{
    public class ConsumerService(ISportNewsRepository repository) : BackgroundService
    {
        private const string Topic = "SportNewsConfirmation";

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = "SportNewsGroup",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            Task.Run(async () =>
            {
                using (var consumer = new ConsumerBuilder<Ignore, ConsumerResult>(config)
                .SetValueDeserializer(new JsonDeserializer<ConsumerResult>())
                .Build())
                {
                    consumer.Subscribe(Topic);

                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var consumerResult = consumer.Consume(TimeSpan.FromSeconds(5));

                        if (consumerResult == null)
                            continue;

                        var result = consumerResult.Message.Value;

                        if (result.Confirmed)
                            await repository.ConfirmSportNews(result.ObjectId, result.UserId, result.ConfirmationTime);

                        consumer.Commit(consumerResult);
                    }
                }
            }, stoppingToken);
            
            return Task.CompletedTask;
        }
    }
}
