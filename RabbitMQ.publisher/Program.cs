using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace RabbitMQ.publisher
{
    public enum LogNames
    {
        Critical = 1,
        Error = 2,
        Warning = 3,
        Info
    }


    class Program
    {
        static void Main(string[] args)
        {
            // Connection RabbitMQ
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://jgamoggd:B57PiXmZthSF6RB_MsuAmnm-tjwFQJ_U@baboon.rmq.cloudamqp.com/jgamoggd");

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.ExchangeDeclare("logs-topic", durable: true, type: ExchangeType.Topic);


            Random rand = new Random();
            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {
                LogNames log1 = (LogNames)rand.Next(1, 5);
                LogNames log2 = (LogNames)rand.Next(1, 5);
                LogNames log3 = (LogNames)rand.Next(1, 5);

                var routeKey = $"{log1}.{log2}.{log3}";
                string message = $"log-type: {log1}-{log2}-{log3}";
                var messageBody = Encoding.UTF8.GetBytes(message);

                // send a message to queue
                // Topic exchange
                channel.BasicPublish("logs-topic", routeKey, null, messageBody);

                Console.WriteLine($"Log gonderilmishdir : {message}");
            });



            Console.ReadLine();
        }
    }
}
