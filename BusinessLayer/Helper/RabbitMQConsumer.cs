using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using Newtonsoft.Json;

namespace BusinessLayer.Helper
{
    /// <summary>
    /// RabbitMQConsumer class listens to the specified queue and processes incoming messages.
    /// </summary>
    public class RabbitMQConsumer
    {
        // RabbitMQ server hostname
        private readonly string _hostName = "localhost";

        // Queue name from which messages will be consumed
        private readonly string _queueName = "ContactAddedQueue";

        /// <summary>
        /// Starts the RabbitMQ consumer to listen for messages on the specified queue.
        /// </summary>
        public void StartConsumer()
        {
            // Create a connection factory with the specified RabbitMQ hostname
            var factory = new ConnectionFactory() { HostName = _hostName };

            // Establish a connection to RabbitMQ
            using var connection = factory.CreateConnection();

            // Create a channel for communication
            using var channel = connection.CreateModel();

            // Declare the queue if it does not already exist
            channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            // Create an event-based consumer
            var consumer = new EventingBasicConsumer(channel);

            // Event triggered when a message is received
            consumer.Received += (model, ea) =>
            {
                // Retrieve message body
                var body = ea.Body.ToArray();

                // Convert byte array to string
                var message = Encoding.UTF8.GetString(body);

                // Log the received message
                Console.WriteLine($"📩 Received message: {message}");
            };

            // Start consuming messages from the queue
            channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

            // Indicate that the consumer is running
            Console.WriteLine("✅ RabbitMQ Consumer Started for ContactAddedQueue. Press [ENTER] to exit.");

            // Keep the application running to continue listening for messages
            Console.ReadLine();
        }
    }
}
