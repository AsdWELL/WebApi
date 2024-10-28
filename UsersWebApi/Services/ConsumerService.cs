using Confluent.Kafka;
using UsersWebApi.Interfaces;
using UsersWebApi.Json;
using UsersWebApi.Models;

namespace UsersWebApi.Services
{
    public class ConsumerService(
        IUserRepository userRepository,
        ProducerService producerService) : BackgroundService
    {
        private const string Topic = "SportNewsRegistration";
        
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = "UserGroup",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest,
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

                        var message = new ProducerMessage();

                        if (await userRepository.AddRegisteredObject(result.UserId))
                        {
                            message.Confirmed = true;
                            message.ObjectId = result.ObjectId;
                            message.UserId = result.UserId;
                            message.ConfirmationTime = DateTime.Now;
                        }

                        consumer.Commit(consumerResult);
                        await producerService.ProduceAsync(stoppingToken, message);
                    }
                }
            }, stoppingToken);

            return Task.CompletedTask;
        }
    }
}
