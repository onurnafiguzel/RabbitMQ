using MassTransit;
using RabbitMQ.ESB.MassTransit.Shared.RequestResponseMessage;

string rabbitMQUri = "amqps://mkckuyqi:T72UpsjX0A1wCShoziWPCM9mK5JgjpGx@woodpecker.rmq.cloudamqp.com/mkckuyqi";
string requestQueue = "request-queue";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
	factory.Host(rabbitMQUri);
});

await bus.StartAsync();

IRequestClient<RequestMessage> request = bus.CreateRequestClient<RequestMessage>(new Uri($"{rabbitMQUri}/{requestQueue}"));

int i = 1;
while (true)
{
	await Task.Delay(200);
	var response = await request.GetResponse<ResponseMessage>(new()
	{
		MessageNo = i,
		Text = $"{i}. request"
	});
	Console.WriteLine($"Responce received: {response.Message.Text}");
}