using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

// Bağlantı oluşturma
ConnectionFactory factory = new();
factory.Uri = new Uri("amqps://mkckuyqi:T72UpsjX0A1wCShoziWPCM9mK5JgjpGx@woodpecker.rmq.cloudamqp.com/mkckuyqi");

// Bağlantıyı aktifleştirme ve kanal açma
using IConnection connection = factory.CreateConnection(); //IDisposable olduğu için using kullanıyorum
using IModel channel = connection.CreateModel();

// Queue oluşturma
channel.QueueDeclare(queue: "example-queue", exclusive: false);

// Queue'dan mesaj okuma
EventingBasicConsumer consumer = new(channel);
channel.BasicConsume(queue: "example-queue", false, consumer);
consumer.Received += (sender, e) =>
{
	// Kuyruğa gelen mesajın işlendiği yerdir!
	// e.body : Kuyruktaki mesajın verisini bütünsel olarak getirecektir. -> e.body.span || e.body.toarray()
	Console.WriteLine(Encoding.UTF8.GetString(e.Body.Span));
};
Console.Read();