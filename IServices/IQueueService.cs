namespace IServices
{
    public interface IQueueService
    {
        Task SendMessageAsync<T>(T message);

        Task ReceiveMessagesAsync();
    }
}
