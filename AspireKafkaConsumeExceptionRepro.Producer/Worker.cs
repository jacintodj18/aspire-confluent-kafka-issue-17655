using Confluent.Kafka;

namespace AspireKafkaConsumeExceptionRepro.Producer;

public class Worker(ILogger<Worker> logger, IProducer<Null, string> producer) : BackgroundService
{
    private const string TopicName = "consume-exception-repro";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Give Kafka and the consumer time to initialize before publishing.
        await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);

        logger.LogInformation("Producing a valid message followed by a poison message to topic {Topic}", TopicName);

        await producer.ProduceAsync(
            TopicName,
            new Message<Null, string> { Value = "GOOD: this message should deserialize" },
            stoppingToken);

        await producer.ProduceAsync(
            TopicName,
            new Message<Null, string> { Value = "BAD: this message will force a ConsumeException" },
            stoppingToken);

        logger.LogInformation("Producer sent both messages. Waiting indefinitely so the host stays alive.");

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}
