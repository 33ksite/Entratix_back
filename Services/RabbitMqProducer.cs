using IServices;
using RabbitMQ.Client;
using System.Data.Common;
using System.Text;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Services
{
    public class RabbitMqProducer : IMessageProducer
    {

        private readonly IRabbitMqConnection _connection;

        public RabbitMqProducer(IRabbitMqConnection connection)
        {
            _connection = connection;
        }

        
        private readonly string _exchangeName = "system_exchange";

        public async Task SendMessageAsync<T>(T message, string routingKey, string queueName)
        {

            using (var channel = _connection.Connection.CreateModel())
            {
                // Declarar el exchange
                channel.ExchangeDeclare(exchange: _exchangeName, type: "direct", durable: true);

                // Declarar la cola especificada
                channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                // Vincular la cola al exchange con la routing key proporcionada
                channel.QueueBind(queue: queueName, exchange: _exchangeName, routingKey: routingKey);

                // Serializar el mensaje y publicarlo
                var messageBody = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(messageBody);

                channel.BasicPublish(exchange: _exchangeName, routingKey: routingKey, basicProperties: null, body: body);

                Console.WriteLine($"Mensaje enviado a la cola '{queueName}' con la routing key '{routingKey}'.");

                await Task.CompletedTask;
            }
        }
    }
}
