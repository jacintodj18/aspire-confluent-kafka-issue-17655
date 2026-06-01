using AspireKafkaConsumeExceptionRepro.Producer;
using Confluent.Kafka;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.AddKafkaProducer<Null, string>("kafka");

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
