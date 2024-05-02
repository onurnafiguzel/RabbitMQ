using RabbitMQ.Client;
using System.Text;

ConnectionFactory factory = new();

// Bağlantı oluşturma -> AMQP.Details.Url
factory.Uri = new Uri("amqps://mkckuyqi:T72UpsjX0A1wCShoziWPCM9mK5JgjpGx@woodpecker.rmq.cloudamqp.com/mkckuyqi");

// Bağlantıyı aktifleştirme ve kanal açma
using IConnection connection = factory.CreateConnection(); //IDisposable olduğu için using kullanıyorum
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(
    exchange: "direct-exchange-example",
    type: ExchangeType.Direct);

while (true)
{
    Console.WriteLine("Mesaj : "); ;
    string mesage = Console.ReadLine();
    byte[] byteMessage = Encoding.UTF8.GetBytes(mesage);

    channel.BasicPublish(
        exchange: "direct-exchange-example",
        routingKey: "direct-queue-example",
        body: byteMessage);
}
