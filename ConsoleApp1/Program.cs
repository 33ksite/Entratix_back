using IServices;
using Services;
using Subscriber;

IRabbitMqConnection rabbitMqConnection = new RabbitMqConnection();
ILogicHandler logicHandler = new LogicHandler();
IRabbitMqConsumerService rabbitMqConsumerService = new RabbitMqConsumerService(rabbitMqConnection, logicHandler);

QueueListener queueListener = new QueueListener(rabbitMqConsumerService);

// Iniciar la escucha
queueListener.Start();

Console.WriteLine("Waiting for messages. Press [enter] to exit.");
Console.ReadLine();

// Liberar recursos
rabbitMqConnection.Dispose();
