using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Text;
using System.Threading;

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


            // her subscriber-e 1-1 mesaj
            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);

            var queueName = channel.QueueDeclare().QueueName;
            var routeKey = "Info.#";

            channel.QueueBind(queueName, "logs-topic", routeKey);

            channel.BasicConsume(queueName, false, consumer);

            Console.WriteLine("listening...");

            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());

                Thread.Sleep(500);
                Console.WriteLine("Gelen Mesaj: " + message);

                File.AppendAllText("log-info.txt", message + '\n');

                // burda mesajin ishlendiyi haqqda xeber veririk.
                channel.BasicAck(e.DeliveryTag, false);
            };



            Console.ReadLine();
        }

    }
}
