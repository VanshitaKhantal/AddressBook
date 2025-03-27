using RabbitMQ.Client;
using System;
using System.Text;
using Newtonsoft.Json;

namespace RepositoryLayer.Helper
{
    /// <summary>
    /// RabbitMQProducer class is responsible for publishing messages to a specified queue.
    /// </summary>
    public class RabbitMQProducer
    {
        // RabbitMQ server hostname
        private readonly string _hostName = "localhost";

        // Queue name where messages will be published
        private readonly string _queueName = "UserRegistrationQueue";

        /// <summary>
        /// Publishes a message to the RabbitMQ queue.
        /// </summary>
        /// <typeparam name="T">Type of the message to be published.</typeparam>
        /// <param name="message">Message object to be serialized and sent to the queue.</param>
        public void PublishMessage<T>(T message)
        {
            // Create a connection factory with the specified RabbitMQ hostname
            var factory = new ConnectionFactory() { HostName = _hostName };

            // Establish a connection to RabbitMQ
            using var connection = factory.CreateConnection();

            // Create a channel for communication
            using var channel = connection.CreateModel();

            // Declare the queue if it does not already exist
            channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            // Serialize the message object to JSON format
            var jsonMessage = JsonConvert.SerializeObject(message);

            // Convert the JSON message to a byte array
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            // Publish the message to the specified queue
            channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);

            // Log the published message
            Console.WriteLine($"📤 Message published: {jsonMessage}");
        }
    }
}
