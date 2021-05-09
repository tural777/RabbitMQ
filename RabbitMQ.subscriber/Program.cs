using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
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

            // olsa da olar, olmasa da
            //channel.QueueDeclare("hello-queue", true, false, false);


            // her subscriber-e 1-1 mesaj
            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);


            // true olanda mesaj sehv ishlense de silinir.
            // false edende ozmuz silinmeyi barede xeber vermeliyik.
            channel.BasicConsume("hello-queue", false, consumer);


            Console.WriteLine("listening...");
            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());

                Thread.Sleep(500);
                Console.WriteLine("Gelen Mesaj: " + message);

                // burda mesajin ishlendiyi haqqda xeber veririk.
                channel.BasicAck(e.DeliveryTag, false);
            };



            Console.ReadLine();
        }

    }
}
