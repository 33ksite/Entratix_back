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
            _rabbitMqConsumerService.StartListening("payments_queue", "email.sent.success");
        }
    }
}
