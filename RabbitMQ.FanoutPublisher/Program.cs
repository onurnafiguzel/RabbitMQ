using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();

factory.Uri = new Uri("amqps://mkckuyqi:T72UpsjX0A1wCShoziWPCM9mK5JgjpGx@woodpecker.rmq.cloudamqp.com/mkckuyqi");

using IConnection connection = factory.CreateConnection(); 
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(
	exchange: "fanout-exchange-example",
	type: ExchangeType.Fanout);

for (int i = 0; i < 100; i++)
{
	await Task.Delay(200);
	byte[] message = Encoding.UTF8.GetBytes($"merhaba {i}");
	channel.BasicPublish(
		exchange: "fanout-exchange-example",
		routingKey: string.Empty,
		body: message
		);
}

Console.Read();