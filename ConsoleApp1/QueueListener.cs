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
            // Solo escuchamos en la cola declarada previamente
            _rabbitMqConsumerService.StartListening("redis_queue", "redis.update");
        }
    }
}
