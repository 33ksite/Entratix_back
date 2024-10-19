using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using IServices;
using System.Text;

namespace Services
{
    public class RabbitMqConsumerService : IRabbitMqConsumerService
    {
        private readonly IRabbitMqConnection _rabbitMqConnection;
        private readonly ILogicHandler _logicHandler;

        public RabbitMqConsumerService(IRabbitMqConnection rabbitMqConnection, ILogicHandler logicHandler)
        {
            _rabbitMqConnection = rabbitMqConnection;
            _logicHandler = logicHandler;
        }

        public void StartListening(string queueName, string routingKey)
        {
            using var channel = _rabbitMqConnection.Connection.CreateModel();

            // No es necesario volver a declarar la cola aquí
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, args) =>
            {
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logicHandler.HandleMessage(queueName, routingKey, message);
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            Console.WriteLine($"Listening on queue: {queueName} with routing key: {routingKey}");
        }
    }
}
