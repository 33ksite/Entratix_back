using Services;
using RabbitMQ.Client;

public class RabbitMqConnection : IRabbitMqConnection, IDisposable
{
    private IConnection? _connection;
    public IConnection Connection => _connection!;

    public RabbitMqConnection()
    {
        InitializeConnection();
    }

    private readonly string _hostname = "localhost";
    private readonly string _username = "user";
    private readonly string _password = "password";

    private void InitializeConnection()
    {
        var factory = new ConnectionFactory
        {
            HostName = _hostname,
            UserName = _username,
            Password = _password
        };

        _connection = factory.CreateConnection();
    }

    public void Dispose()
    {
        _connection?.Dispose();
    }
}
