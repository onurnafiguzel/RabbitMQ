using MassTransit;
using RabbitMQ.ESB.MassTransit.WorkerService.Publisher.Services;

namespace RabbitMQ.ESB.MassTransit.WorkerService.Publisher
{
	public class Program
	{
		public static void Main(string[] args)
		{
			IHost host = Host.CreateDefaultBuilder(args)
				.ConfigureServices(services =>
				{
					services.AddMassTransit(configurator =>
					{
						configurator.UsingRabbitMq((context, _configurator) =>
						{
							_configurator.Host("amqps://mkckuyqi:T72UpsjX0A1wCShoziWPCM9mK5JgjpGx@woodpecker.rmq.cloudamqp.com/mkckuyqi");
						});
					});

					services.AddHostedService<PublishMessageService>(provider =>
					{
						using IServiceScope scope = provider.CreateScope();
						IPublishEndpoint publishEndpoint = scope.ServiceProvider.GetService<IPublishEndpoint>();
						return new(publishEndpoint);
					});
				})
				.Build();

			host.Run();
		}
	}
}