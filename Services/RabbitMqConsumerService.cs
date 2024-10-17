using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using IServices;
using System.Text;

namespace Services
{
    public class RabbitMqConsumerService : IRabbitMqConsumerService
    {
        private readonly IRabbitMqConnection _rabbitMqConnection;
        private readonly ILogicHandler _logicHandler;  // Interfaz que maneja la lógica por cola/routingKey

        public RabbitMqConsumerService(IRabbitMqConnection rabbitMqConnection, ILogicHandler logicHandler)
        {
            _rabbitMqConnection = rabbitMqConnection;
            _logicHandler = logicHandler;
        }

        public void StartListening(string queueName, string routingKey)
        {
            using var channel = _rabbitMqConnection.Connection.CreateModel();

            // Declarar el exchange y la cola
            channel.ExchangeDeclare(exchange: "system_exchange", type: "direct", durable: true);
            channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

            // Vincular la cola al exchange con la clave de enrutamiento
            channel.QueueBind(queue: queueName, exchange: "system_exchange", routingKey: routingKey);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, args) =>
            {
                var body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                // Enrutar la lógica según la clave de enrutamiento y la cola
                _logicHandler.HandleMessage(queueName, routingKey, message);
            };

            // Consumir mensajes de la cola
            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

            // Mantener la escucha activa
            Console.WriteLine($"Listening on queue: {queueName} with routing key: {routingKey}");
        }
    }
}
