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

        // Tiempo de espera entre reintentos (en milisegundos)
        private readonly int _retryInterval = 5000; // 5 segundos

        public RabbitMqConsumerService(IRabbitMqConnection rabbitMqConnection, ILogicHandler logicHandler)
        {
            _rabbitMqConnection = rabbitMqConnection;
            _logicHandler = logicHandler;
        }

        public void StartListening(string queueName, string routingKey)
        {
            AttemptToConsume(queueName, routingKey);
        }

        private void AttemptToConsume(string queueName, string routingKey)
        {
            try
            {
                using var channel = _rabbitMqConnection.Connection.CreateModel();

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (sender, args) =>
                {
                    var body = args.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    _logicHandler.HandleMessage(queueName, routingKey, message);
                };

                // Intentar consumir de la cola
                channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
                Console.WriteLine($"Listening on queue: {queueName} with routing key: {routingKey}");

                // Mantener la escucha activa
                while (true)
                {
                    // Mantener el hilo activo para evitar que la conexión se cierre
                    Task.Delay(1000).Wait();
                }
            }
            catch (Exception ex)
            {
                // Manejar el error si la cola no está disponible (404)
                if (ex.Message.Contains("NOT_FOUND"))
                {
                    Console.WriteLine($"Error: La cola '{queueName}' no existe aún. Reintentando en {_retryInterval / 1000} segundos...");
                    Task.Delay(_retryInterval).Wait(); // Esperar antes de volver a intentar
                    AttemptToConsume(queueName, routingKey); // Reintentar
                }
                else
                {
                    Console.WriteLine($"Error en la conexión a RabbitMQ: {ex.Message}. Reintentando en {_retryInterval / 1000} segundos...");
                    Task.Delay(_retryInterval).Wait(); // Esperar antes de volver a intentar
                    AttemptToConsume(queueName, routingKey); // Reintentar
                }
            }
        }
    }
}
