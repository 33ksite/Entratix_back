namespace IServices
{
    public interface ILogicHandler
    {
        void HandleMessage(string queueName, string routingKey, string message);
    }
}
