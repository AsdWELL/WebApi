using Confluent.Kafka;
using UsersWebApi.Models;
using UsersWebApi.Json;

namespace UsersWebApi.Services
{
    public class ProducerService
    {
        private const string Topic = "SportNewsConfirmation";

        public async Task ProduceAsync(CancellationToken stoppingToken, ProducerMessage message)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092",
                AllowAutoCreateTopics = true,
                Acks = Acks.All
            };

            using (var producer = new ProducerBuilder<Null, ProducerMessage>(config)
                .SetValueSerializer(new JsonSerializer<ProducerMessage>())
                .Build())
            {
                await producer.ProduceAsync(topic: Topic,
                    new Message<Null, ProducerMessage>
                    {
                        Value = message
                    }, stoppingToken);

                producer.Flush(stoppingToken);
            }
        }
    }
}
