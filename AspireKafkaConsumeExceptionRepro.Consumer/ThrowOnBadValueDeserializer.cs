using System.Text;
using Confluent.Kafka;

namespace AspireKafkaConsumeExceptionRepro.Consumer;

internal sealed class ThrowOnBadValueDeserializer : IDeserializer<string>
{
    public string Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        if (isNull)
        {
            return string.Empty;
        }

        var value = Encoding.UTF8.GetString(data);

        if (value.StartsWith("BAD:", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Poison payload detected by custom deserializer.");
        }

        return value;
    }
}
