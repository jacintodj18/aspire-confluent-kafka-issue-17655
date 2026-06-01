using Confluent.Kafka;

namespace AspireKafkaConsumeExceptionRepro.Consumer;

public class Worker(ILogger<Worker> logger, IConsumer<Ignore, string> consumer) : BackgroundService
{
    private const string TopicName = "consume-exception-repro";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        consumer.Subscribe(TopicName);

        logger.LogInformation("Consumer subscribed to topic {Topic}", TopicName);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = consumer.Consume(stoppingToken);
                logger.LogInformation(
                    "Consumed message at {TopicPartitionOffset}. Payload: {Value}",
                    result.TopicPartitionOffset,
                    result.Message.Value);
            }
            catch (ConsumeException ex)
            {
                logger.LogWarning(
                    ex,
                    "ConsumeException captured. ErrorCode: {ErrorCode}, Reason: {Reason}",
                    ex.Error.Code,
                    ex.Error.Reason);
            }
        }

        consumer.Close();

        await Task.CompletedTask;
    }
}
