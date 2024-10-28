using Confluent.Kafka;
using SportNewsWebApi.Json;
using SportNewsWebApi.Models;

namespace SportNewsWebApi.Services
{
    public class ProducerService
    {
        private const string Topic = "SportNewsRegistration";

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
