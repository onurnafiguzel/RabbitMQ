using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();

factory.Uri = new Uri("amqps://mkckuyqi:T72UpsjX0A1wCShoziWPCM9mK5JgjpGx@woodpecker.rmq.cloudamqp.com/mkckuyqi");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(
	exchange: "topic-exchange-example",
	type: ExchangeType.Topic
	);

Console.WriteLine("Dinlenecek Topic formatını belirtiniz");
string topic = Console.ReadLine();

string queueName = channel.QueueDeclare().QueueName;
channel.QueueBind(
	exchange: "topic-exchange-example",
	queue: queueName,
	routingKey:topic
	);

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
	queue: queueName,
	autoAck: true,
	consumer: consumer
	);

consumer.Received += (sender, e) =>
{
	string message = Encoding.UTF8.GetString(e.Body.Span);
	Console.WriteLine(message);
};

Console.Read();