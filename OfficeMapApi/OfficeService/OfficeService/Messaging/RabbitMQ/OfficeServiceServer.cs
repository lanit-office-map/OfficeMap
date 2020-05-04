using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using OfficeService.Messaging.RabbitMQ.Interface;
using OfficeService.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;

namespace OfficeService.Messaging.RabbitMQ
{
    public class OfficeServiceServer : IHostedService
    {
        private readonly IRabbitMQPersistentConnection persistentConnection;
        private readonly IOfficeService officeService;
        private readonly ILogger<OfficeServiceServer> logger;
        private const string queue = "officeguid_queue";

        public OfficeServiceServer(
            [FromServices] IRabbitMQPersistentConnection persistentConnection,
            [FromServices] IOfficeService officeService,
            [FromServices] ILogger<OfficeServiceServer> logger)
        {
            this.persistentConnection = persistentConnection;
            this.officeService = officeService;
            this.logger = logger;
            CreateConsumerChannel(queue);
            Console.WriteLine("OfficeService: created an " + queue);
        }

        public void CreateConsumerChannel(string queueName)
        {
            if (!persistentConnection.IsConnected)
            {
                persistentConnection.TryConnect();
            }

            var channel = persistentConnection.CreateModel();
            channel.QueueDeclare(queue: queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(queue: queueName,
                                 autoAck: false, 
                                 consumer: consumer);

            consumer.Received += (model, ea) =>
            {
                ReceivedEvent(model, ea, channel);
            };



            
        }
        public void ReceivedEvent(object? model, BasicDeliverEventArgs ea, IModel channel)
        {
            var body = ea.Body;
            var props = ea.BasicProperties;
            var replyProps = channel.CreateBasicProperties();
            replyProps.CorrelationId = props.CorrelationId;

            var message = Encoding.UTF8.GetString(body.ToArray());
            Guid officeguid = Guid.Parse(message);
            Console.WriteLine("OfficeGUID: " + message);
            var office = officeService.GetAsync(officeguid).Result;

            var response = JsonConvert.SerializeObject(office);
            var responseBytes = Encoding.UTF8.GetBytes(response);

            channel.BasicPublish(exchange: "",
                                 routingKey: props.ReplyTo,
                                 basicProperties: replyProps,
                                 body: responseBytes);

            channel.BasicAck(deliveryTag: ea.DeliveryTag,
                             multiple: false);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
