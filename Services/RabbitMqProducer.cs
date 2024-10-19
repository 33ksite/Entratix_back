using IServices;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Services
{
    public class RabbitMqProducer : IMessageProducer
    {
        private readonly IRabbitMqConnection _connection;
        private readonly string _exchangeName = "system_exchange";

        public RabbitMqProducer(IRabbitMqConnection connection)
        {
            _connection = connection;
        }

        public async Task SendMessageAsync<T>(T message, string routingKey, string queueName)
        {
            using (var channel = _connection.Connection.CreateModel())
            {
                // Declaramos el exchange (ya está declarado en RabbitMqSetup, pero no hay problema con volver a declarar el exchange)
                channel.ExchangeDeclare(exchange: _exchangeName, type: "direct", durable: true);

                // Serializar el mensaje y publicarlo
                var messageBody = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(messageBody);

                channel.BasicPublish(exchange: _exchangeName, routingKey: routingKey, basicProperties: null, body: body);

                Console.WriteLine($"Mensaje enviado al exchange '{_exchangeName}' con la routing key '{routingKey}'.");
                await Task.CompletedTask;
            }
        }
    }
}
