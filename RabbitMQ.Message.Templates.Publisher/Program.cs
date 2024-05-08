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

//byte[] message = Encoding.UTF8.GetBytes("merhaba");
//channel.BasicPublish(
//	exchange: string.Empty,
//	routingKey: queueName,
//	body: message
//	 );

#endregion
#region Publish/Subscribe (Pub/Sub) Tasarımı
//string exchangeName = "example-pub-sub-exchane";

//channel.ExchangeDeclare(
//	exchange: exchangeName,
//	type: ExchangeType.Fanout);

//for (int i = 0; i < 100; i++)
//{
//	byte[] message = Encoding.UTF8.GetBytes("merhaba" + i);
//	channel.BasicPublish(
//		exchange: exchangeName,
//		routingKey: string.Empty,
//		body: message);
//}

#endregion
#region Work Queue (İş kuyruğu) Tasarımı
//string queueName = "example-work-queue";
//channel.QueueDeclare(
//	queue: queueName,
//	durable: false,
//	exclusive: false,
//	autoDelete: false);

//for (int i = 0; i < 100; i++)
//{
//	byte[] message = Encoding.UTF8.GetBytes("merhaba" + i);
//	channel.BasicPublish(
//		exchange: string.Empty,
//		routingKey: queueName,
//		body: message);
//}
#endregion
#region Reuqest/Response Tasarımı
string requestQueueName = "example-request-response-queue";
channel.QueueDeclare(
	queue: requestQueueName,
	durable: false,
	exclusive: false,
	autoDelete: false); ;

string replyQueueName = channel.QueueDeclare().QueueName;
string correlationId = Guid.NewGuid().ToString();

#region Request Mesajını Oluşturma ve Gönderme
IBasicProperties properties = channel.CreateBasicProperties();
properties.CorrelationId = correlationId;
properties.ReplyTo = replyQueueName;

for (int i = 0; i < 10; i++)
{
	byte[] message = Encoding.UTF8.GetBytes("merhaba" + i);
	channel.BasicPublish(
		exchange: string.Empty,
		routingKey: requestQueueName,
		body: message,
		basicProperties: properties
		);
}
#endregion
#region Response Kuyruğu Dinleme
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(
	queue: replyQueueName,
	autoAck: true,
	consumer: consumer);

consumer.Received += (sender, e) =>
{
	if (e.BasicProperties.CorrelationId == correlationId)
		Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
};

#endregion
#endregion


Console.Read();
