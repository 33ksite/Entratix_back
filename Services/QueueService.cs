using IServices;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Services
{
    public class QueueService : IQueueService
    {
        private readonly string _hostname = "localhost";
        private readonly string _username = "user";
        private readonly string _password = "password";
        private readonly string _exchangeName = "system_exchange";  // Nombre del exchange común

        public async Task SendMessageAsync<T>(T message, string routingKey, string queueName)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostname,
                UserName = _username,
                Password = _password
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
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

        public async Task ReceiveMessagesAsync(string routingKey, string queueName)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _hostname,
                UserName = _username,
                Password = _password
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                // Declarar el exchange y la cola
                channel.ExchangeDeclare(exchange: _exchangeName, type: "direct", durable: true);
                channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueBind(queue: queueName, exchange: _exchangeName, routingKey: routingKey);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    // Procesar el mensaje
                    Console.WriteLine($"Mensaje recibido de RabbitMQ: {message}");
                };

                channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

                // Mantener el receptor activo
                while (true)
                {
                    await Task.Delay(1000);
                }
            }
        }

    }
}
