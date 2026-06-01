var builder = DistributedApplication.CreateBuilder(args);

var kafka = builder
	.AddKafka("kafka")
	.WithKafkaUI();

builder.AddProject<Projects.AspireKafkaConsumeExceptionRepro_Producer>("producer")
	.WithReference(kafka)
	.WaitFor(kafka);

builder.AddProject<Projects.AspireKafkaConsumeExceptionRepro_Consumer>("consumer")
	.WithReference(kafka)
	.WaitFor(kafka);

builder.Build().Run();
