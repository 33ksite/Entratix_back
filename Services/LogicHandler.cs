using IServices;

namespace Services
{
    public class LogicHandler : ILogicHandler
    {
        public void HandleMessage(string queueName, string routingKey, string message)
        {
            // Aquí puedes decidir qué hacer en función del nombre de la cola y la clave de enrutamiento
            if (queueName == "orders" && routingKey == "order.created")
            {
                Console.WriteLine($"Processing 'order.created' for queue {queueName}: {message}");
                // Agregar lógica específica para "order.created"
            }
            else if (queueName == "payments" && routingKey == "payment.success")
            {
                Console.WriteLine($"Processing 'payment.success' for queue {queueName}: {message}");
                // Agregar lógica específica para "payment.success"
            }
            else
            {
                Console.WriteLine($"Received message from {queueName} with key {routingKey}: {message}");
            }
        }
    }
}
