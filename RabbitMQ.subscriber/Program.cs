using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMQ.subscriber
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

            // olsa da olar, olmasa da
            //channel.QueueDeclare("hello-queue", true, false, false);

            var consumer = new EventingBasicConsumer(channel);

            channel.BasicConsume("hello-queue", true, consumer);

            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());

                Console.WriteLine("Gelen Mesaj: " + message);
            };



            Console.ReadLine();
        }
      
    }
}
