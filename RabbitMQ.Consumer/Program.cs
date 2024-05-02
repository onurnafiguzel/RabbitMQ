using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

// Bağlantı oluşturma
ConnectionFactory factory = new();
factory.Uri = new Uri("amqps://mkckuyqi:T72UpsjX0A1wCShoziWPCM9mK5JgjpGx@woodpecker.rmq.cloudamqp.com/mkckuyqi");

// Bağlantıyı aktifleştirme ve kanal açma
using IConnection connection = factory.CreateConnection(); //IDisposable olduğu için using kullanıyorum
using IModel channel = connection.CreateModel();

// 1.adım
channel.ExchangeDeclare(
	exchange: "direct-exchange-example",
	type: ExchangeType.Direct
	);

// 2.adım
string queueName = channel.QueueDeclare().QueueName;


// 3.adım
channel.QueueBind(
	queue: queueName,
	exchange: "direct-exchange-example",
	routingKey: "direct-queue-example");

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

// 1. Adım: Publisher'daki exchange ile birebir aynı isim ve type'a sahip bir exchange tanımlanmalıdır!
// 2. Adım: Publisher tarafından routing key'de bulunan değerdeki kuyruğa gönderilen mesajları kendi oluşturduğumuz kuyruğa yönlendirerek tüketmemz gerekmektedir. Bunun için öncelikle bir kuyruk oluşturmaldıır!
// 3. Adım: 