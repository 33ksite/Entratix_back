namespace IServices
{
    public interface IMessageProducer
    {
        Task SendMessageAsync<T>(T message, string routingKey, string queueName);

    }
}
