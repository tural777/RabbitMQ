using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace RabbitMQ.publisher
{
    class Program
    {
        static void Main(string[] args)
        {
            // Connection RabbitMQ
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://jgamoggd:B57PiXmZthSF6RB_MsuAmnm-tjwFQJ_U@baboon.rmq.cloudamqp.com/jgamoggd");

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.QueueDeclare("hello-queue", true, false, false);

            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {
                string message = $"Message {x}";

                var messageBody = Encoding.UTF8.GetBytes(message);

                // send a message to queue
                // default exchange (the exchange name is the same as the queue name)
                channel.BasicPublish(string.Empty, "hello-queue", null, messageBody);

                Console.WriteLine($"Mesaj gonderilmishdir : {message}");
            });



            Console.ReadLine();
        }
    }
}
