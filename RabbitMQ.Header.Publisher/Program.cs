using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();

factory.Uri = new Uri("amqps://mkckuyqi:T72UpsjX0A1wCShoziWPCM9mK5JgjpGx@woodpecker.rmq.cloudamqp.com/mkckuyqi");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(
	exchange: "header-exchange-example",
	type: ExchangeType.Headers
	);

for (int i = 0; i < 100; i++)
{
	await Task.Delay(200);
	byte[] message = Encoding.UTF8.GetBytes($"merhaba {i}");
    Console.WriteLine("Header values değerini giriniz");
	string value = Console.ReadLine();

	IBasicProperties basicProperties =  channel.CreateBasicProperties();

	basicProperties.Headers = new Dictionary<string, object>
	{
		["no"] = value
	};

	channel.BasicPublish(
		exchange: "header-exchange-exampler",
		routingKey: string.Empty,
		body: message,
		basicProperties: basicProperties
		);
}

Console.Read();