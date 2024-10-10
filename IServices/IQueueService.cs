namespace IServices
{
    public interface IQueueService
    {
        Task SendMessageAsync<T>(T message, string routingKey, string queueName);

        Task ReceiveMessagesAsync(string routingKey, string queueName);
    }
}
