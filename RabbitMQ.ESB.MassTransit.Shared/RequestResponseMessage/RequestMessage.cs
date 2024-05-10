namespace RabbitMQ.ESB.MassTransit.Shared.RequestResponseMessage;

public record RequestMessage
{
	public string Text { get; set; }
    public int MessageNo { get; set; }

}
