using IServices;

namespace Services
{
    public class LogicHandler : ILogicHandler
    {
        
        private readonly Dictionary<(string queueName, string routingKey), Action<string>> _handlers;

        public LogicHandler()
        {
            // Inicializar el diccionario con las acciones correspondientes
            _handlers = new Dictionary<(string queueName, string routingKey), Action<string>>
            {
                { ("payments_queue", "email.sent.success"), EmailSent },
                
            };
        }

        // Método que se llama para manejar el mensaje
        public void HandleMessage(string queueName, string routingKey, string message)
        {
            if (_handlers.TryGetValue((queueName, routingKey), out var handler))
            {
                handler(message); 
            }
            else
            {
                Console.WriteLine($"No handler found for queue '{queueName}' and routing key '{routingKey}'. Message: {message}");
            }
        }

        
        private void EmailSent(string message)
        {
            Console.WriteLine($"Processing 'email.sent.success': {message}");
        }

    }
}
