using MassTransit;
using RabbitMQ.ESB.MassTransit.Shared.Messages;

namespace RabbitMQ.ESB.MassTransit.WorkerService.Consumer.Consumers;

public class ExampleMessageConsumer : IConsumer<IMessage>
{
	public async Task Consume(ConsumeContext<IMessage> context)
	{
		Console.WriteLine($"Gelen mesahj: {context.Message.Text}");
	}
}
