using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();

factory.Uri = new Uri("amqps://mkckuyqi:T72UpsjX0A1wCShoziWPCM9mK5JgjpGx@woodpecker.rmq.cloudamqp.com/mkckuyqi");

using IConnection connection = factory.CreateConnection();
using IModel channel = connection.CreateModel();

#region P2P (Point-toPoint) Tasarımı
//string queueName = "example-p2p-queue";
//channel.QueueDeclare(
//	queue: queueName,
//	durable: false,
//	exclusive: false,
//	autoDelete: false
//	);

//EventingBasicConsumer consumer = new(channel);
//channel.BasicConsume(
//	queue: queueName,
//	autoAck: false,
//	consumer: consumer
//	);

//consumer.Received += (sender, e) =>
//{
//	Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
//};
#endregion
#region Publish/Subscribe (Pub/Sub) Tasarımı
//string exchangeName = "example-pub-sub-exchane";

//channel.ExchangeDeclare(
//	exchange: exchangeName,
//	type: ExchangeType.Fanout);

//string queueName = channel.QueueDeclare().QueueName;
//channel.QueueBind(
//	queue: queueName,
//	exchange: exchangeName,
//	routingKey: string.Empty);

//EventingBasicConsumer consumer = new(channel);
//channel.BasicConsume(
//	queue: queueName,
//	autoAck: false,
//	consumer: consumer
//	);

//// İsteğe bağlı channel.BasicOps ile ölçeklendirme yapılabilir.
//consumer.Received += (sender, e) =>
//{
//	Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
//};
#endregion
#region Work Queue (İş kuyruğu) Tasarımı
//string queueName = "example-work-queue";
//channel.QueueDeclare(
//	queue: queueName,
//	durable: false,
//	exclusive: false,
//	autoDelete: false);

//EventingBasicConsumer consumer = new(channel);
//channel.BasicConsume(
//	queue: queueName,
//	autoAck: true,
//	consumer: consumer
//	);

//channel.BasicQos(
//	prefetchCount: 1,
//	prefetchSize: 0,
//	global: false
//	);

//consumer.Received += (sender, e) =>
//{
//	Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span);
//};
#endregion
#region Reuqest/Response Tasarımı
string requestQueueName = "example-request-response-queue";
channel.QueueDeclare(
	queue: requestQueueName,
	durable: false,
	exclusive: false,
	autoDelete: false); ;

EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
	queue: requestQueueName,
	autoAck: true,
	consumer: consumer);

consumer.Received += (sender, e) =>
{
	string message = Encoding.UTF8.GetString(e.Body.Span);
	Console.WriteLine(message);
	//.. other works
	byte[] responseMessage = Encoding.UTF8.GetBytes($"İşlem tamamlandı.:  {message}");
	IBasicProperties properties = channel.CreateBasicProperties();
	properties.CorrelationId = e.BasicProperties.CorrelationId;

	channel.BasicPublish(
		exchange: string.Empty,
		routingKey: e.BasicProperties.ReplyTo,
		basicProperties: properties,
		body: responseMessage);
};

#endregion

Console.Read();
