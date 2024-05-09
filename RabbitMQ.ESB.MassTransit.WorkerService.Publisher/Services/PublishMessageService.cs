﻿
using MassTransit;
using RabbitMQ.ESB.MassTransit.Shared.Messages;

namespace RabbitMQ.ESB.MassTransit.WorkerService.Publisher.Services;

public class PublishMessageService : BackgroundService
{
	readonly IPublishEndpoint _publishEndpoint;
	public PublishMessageService(IPublishEndpoint publishEndpoint)
	{
		_publishEndpoint = publishEndpoint;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		int i = 0;
		while (true)
		{
			ExampleMessage message = new()
			{
				Text = $"{++i}. mesaj"
			};

			// publish bu exchange'e bağlı olan bütün kuyruklara mesaj gönderir.
			await _publishEndpoint.Publish(message);
		}
	}
}
