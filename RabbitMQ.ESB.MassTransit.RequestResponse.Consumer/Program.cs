using MassTransit;
using RabbitMQ.ESB.MassTransit.RequestResponse.Consumer.Consumers;

string rabbitMQUri = "amqps://mkckuyqi:T72UpsjX0A1wCShoziWPCM9mK5JgjpGx@woodpecker.rmq.cloudamqp.com/mkckuyqi";
string requestQueue = "request-queue";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
	factory.Host(rabbitMQUri);

	factory.ReceiveEndpoint(requestQueue, endpoint =>
	{
		endpoint.Consumer<RequestMessageConsumer>();
	});
});

await bus.StartAsync();

Console.Read();