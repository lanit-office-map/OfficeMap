using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using WorkplaceService.Messaging.RabbitMQ.Interface;
using WorkplaceService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WorkplaceService.Messaging.RabbitMQ
{
    public class WorkplaceServiceServer
    {
        private readonly IRabbitMQPersistentConnection persistentConnection;
        private readonly IWorkplaceService workplaceService;
        private readonly ILogger<WorkplaceServiceServer> logger;
        private const string queue = "workplaceguid_queue";

        public WorkplaceServiceServer(
            [FromServices] IRabbitMQPersistentConnection persistentConnection,
            [FromServices] IWorkplaceService workplaceService,
            [FromServices] ILogger<WorkplaceServiceServer> logger)
        {
            this.persistentConnection = persistentConnection;
            this.workplaceService = workplaceService;
            this.logger = logger;
            CreateConsumerChannel(queue);
            Console.WriteLine("WorkplaceService: created an " + queue);
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

        private void ReceivedEvent(object? model, BasicDeliverEventArgs ea, IModel channel)
        {
            var body = ea.Body;
            var props = ea.BasicProperties;
            var replyProps = channel.CreateBasicProperties();
            replyProps.CorrelationId = props.CorrelationId;

            var message = Encoding.UTF8.GetString(body.ToArray());
            var workplaceguid = Guid.Parse(message);
            Console.WriteLine("OfficeGUID: " + message);
            var workplace = workplaceService.GetAsync(workplaceguid).Result;

            var response = JsonConvert.SerializeObject(workplace);
            var responseBytes = Encoding.UTF8.GetBytes(response);

            channel.BasicPublish(exchange: "",
                                 routingKey: props.ReplyTo,
                                 basicProperties: replyProps,
                                 body: responseBytes);

            channel.BasicAck(deliveryTag: ea.DeliveryTag,
                             multiple: false);
        }
    }
}
