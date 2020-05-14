using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SpaceService.Controllers;
using SpaceService.Models;
using SpaceService.RabbitMQ.Interface;
using SpaceService.Services.Interfaces;
using System;
using System.Text;

namespace SpaceService.Servers
{
    public class SpaceServiceServer
    {
        private readonly IRabbitMQPersistentConnection persistentConnection;
        private readonly ISpacesService spaceService;
        private readonly ILogger<SpaceServiceServer> logger;
        private readonly SpaceController spaceController;
        private const string queue = "spaceservice_queue";

        private async void ReceivedEvent(object? model, BasicDeliverEventArgs ea, IModel channel)
        {
            var body = ea.Body;
            var props = ea.BasicProperties;
            var replyProps = channel.CreateBasicProperties();
            replyProps.CorrelationId = props.CorrelationId;

            var message = Encoding.UTF8.GetString(body.ToArray());
            WorkplaceRequest guids = JsonConvert.DeserializeObject<WorkplaceRequest>(message);
            Console.WriteLine("SpaceGuid: " + guids.SpaceGuid);
            Console.WriteLine("OfficeGuid: " + guids.OfficeGuid);
            var space = (await spaceController.GetSpace(guids.OfficeGuid, guids.SpaceGuid)).Value;
            if (space == null)
            {
                var feedback = new Error();
            }

            var response = space.SpaceId.ToString();
            var responseBytes = Encoding.UTF8.GetBytes(response);

            channel.BasicPublish(exchange: "guids",
                                 routingKey: props.ReplyTo,
                                 basicProperties: replyProps,
                                 body: responseBytes);

            channel.BasicAck(deliveryTag: ea.DeliveryTag,
                             multiple: false);
        }

        public SpaceServiceServer(
            [FromServices] IRabbitMQPersistentConnection persistentConnection,
            [FromServices] ISpacesService spaceService,
            [FromServices] SpaceController spaceController,
            [FromServices] ILogger<SpaceServiceServer> logger)
        {
            this.persistentConnection = persistentConnection;
            this.spaceService = spaceService;
            this.spaceController = spaceController;
            this.logger = logger;
            CreateConsumerChannel(queue);
            Console.WriteLine("SpaceService: created an " + queue);
        }

        public void CreateConsumerChannel(string queueName)
        {
            if (!persistentConnection.IsConnected)
            {
                persistentConnection.TryConnect();
            }

            var channel = persistentConnection.CreateModel();
            channel.ExchangeDeclare("guids", ExchangeType.Direct);
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
        
    }
}
