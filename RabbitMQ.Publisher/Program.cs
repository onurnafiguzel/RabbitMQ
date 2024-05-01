using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();

// Bağlantı oluşturma -> AMQP.Details.Url
factory.Uri = new Uri("amqps://mkckuyqi:T72UpsjX0A1wCShoziWPCM9mK5JgjpGx@woodpecker.rmq.cloudamqp.com/mkckuyqi");

// Bağlantıyı aktifleştirme ve kanal açma
using IConnection connection = factory.CreateConnection(); //IDisposable olduğu için using kullanıyorum
using IModel channel = connection.CreateModel();

// Queue oluşturma -> exclusive=true başka exchange erişemez demek
channel.QueueDeclare(queue: "example-queue", exclusive: false);

// Queue'ya mesaj gönderme
// RabbitMQ kuyruğa atacağı mesajları byte türünden kabul eder.
byte[] message = Encoding.UTF8.GetBytes("merhaba");
channel.BasicPublish(exchange: "", routingKey: "example-queue", body: message); // exhange default 'direct'

Console.Read();
