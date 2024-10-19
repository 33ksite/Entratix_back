using RabbitMQ.Client;

namespace Services
{
    public class RabbitMqSetup
    {
        private readonly IRabbitMqConnection _connection;

        public RabbitMqSetup(IRabbitMqConnection connection)
        {
            _connection = connection;
        }

        public void Initialize()
        {
            using var channel = _connection.Connection.CreateModel();

            // Declarar el exchange central
            channel.ExchangeDeclare(exchange: "system_exchange", type: "direct", durable: true);

            // Declarar las colas solo una vez en el sistema
            channel.QueueDeclare(queue: "payments_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueDeclare(queue: "redis_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

            // Vincular las colas al exchange con las routing keys correspondientes
            channel.QueueBind(queue: "payments_queue", exchange: "system_exchange", routingKey: "email.sent.success");
            channel.QueueBind(queue: "payments_queue", exchange: "system_exchange", routingKey: "payment.success");
            channel.QueueBind(queue: "redis_queue", exchange: "system_exchange", routingKey: "ticket.validation");
        }
    }
}
