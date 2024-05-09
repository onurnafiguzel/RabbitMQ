using MassTransit;
using RabbitMQ.ESB.MassTransit.WorkerService.Consumer.Consumers;

namespace RabbitMQ.ESB.MassTransit.WorkerService.Consumer
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			IHost host = Host.CreateDefaultBuilder(args)
				.ConfigureServices(services =>
				{
					services.AddMassTransit(configurator =>
					{
						configurator.AddConsumer<ExampleMessageConsumer>();

						configurator.UsingRabbitMq((context, _configurator) =>
						{
							_configurator.Host("amqps://mkckuyqi:T72UpsjX0A1wCShoziWPCM9mK5JgjpGx@woodpecker.rmq.cloudamqp.com/mkckuyqi");

							_configurator.ReceiveEndpoint("example-message-queue", e => e.ConfigureConsumer<ExampleMessageConsumer>(context));
						});
					});
				})
				.Build();

			await host.RunAsync();
		}
	}
}