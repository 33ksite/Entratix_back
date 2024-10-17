namespace IServices
{
    public interface IRabbitMqConsumerService
    {
        void StartListening(string queueName, string routingKey);
    }
}
