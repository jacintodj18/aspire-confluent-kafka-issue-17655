using AspireKafkaConsumeExceptionRepro.Consumer;
using Confluent.Kafka;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.AddKafkaConsumer<Ignore, string>(
	"kafka",
	settings =>
	{
		settings.Config.GroupId = "consume-exception-repro-group";
		settings.Config.AutoOffsetReset = AutoOffsetReset.Earliest;
	},
	consumerBuilder =>
	{
		consumerBuilder.SetValueDeserializer(new ThrowOnBadValueDeserializer());
	});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
