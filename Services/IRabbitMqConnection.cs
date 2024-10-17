
using RabbitMQ.Client;

namespace Services
{
    public interface IRabbitMqConnection : IDisposable
    {
        IConnection Connection { get; }
    }
}
