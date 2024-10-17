using IServices;

namespace Subscriber
{

    public class QueueListener
    {
        private readonly IRabbitMqConsumerService _rabbitMqConsumerService;

        public QueueListener(IRabbitMqConsumerService rabbitMqConsumerService)
        {
            _rabbitMqConsumerService = rabbitMqConsumerService;
        }

        public void Start()
        {
            // Configurar diferentes colas y claves de enrutamiento
            _rabbitMqConsumerService.StartListening("redis", "redis.update");
        }
    }
}
