﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

ConnectionFactory factory = new();

factory.Uri = new Uri("amqps://mkckuyqi:T72UpsjX0A1wCShoziWPCM9mK5JgjpGx@woodpecker.rmq.cloudamqp.com/mkckuyqi");

using IConnection connection = factory.CreateConnection(); 
using IModel channel = connection.CreateModel();

channel.ExchangeDeclare(
	exchange: "fanout-exchange-example",
	type: ExchangeType.Fanout);

Console.WriteLine("Kuyruk adını giriniz: ");
string queueName = Console.ReadLine();

channel.QueueDeclare(
	queue: queueName,
	exclusive: false);

channel.QueueBind(
	queue: queueName,
	exchange: "fanout-exchange-example",
	routingKey: string.Empty
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