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
using System.Collections.Specialized;
using System.Text;

namespace SpaceService.Servers
{
    public class SpaceServiceServer
    {
        #region private fields
        private readonly IRabbitMQPersistentConnection persistentConnection;
        private readonly SpaceController spaceController;
        #endregion
        private readonly StringCollection ReplyBindingKeys = new StringCollection()
        {
            "space_data", "space_error"
        };
        private const string ReplyExchange = "replies";
        private const string ReplyQueueName = "SpaceService_ReplyQueue";

        private const string RequestQueueName = "SpaceService_RequestQueue";
        private const string RequestExchange = "requests";
        private const string RequestBindingKey = "SpaceRequest";

        #region Private Methods
        private async void MessageReceived(object? model, BasicDeliverEventArgs ea, IModel channel)
        {
            var InboundMessage = ea.Body;
            var InboundProperties = ea.BasicProperties;

            var ReplyProperties = channel.CreateBasicProperties();
            ReplyProperties.CorrelationId = InboundProperties.CorrelationId;
            ReplyProperties.ReplyTo = InboundProperties.ReplyTo;

            var message = Encoding.UTF8.GetString(InboundMessage.ToArray());
            WorkplaceRequest guids = JsonConvert.DeserializeObject<WorkplaceRequest>(message);
            Console.WriteLine("OfficeGUID received: " + guids.OfficeGuid);
            Console.WriteLine("SpaceGUID received: " + guids.SpaceGuid);

            var space = (await spaceController.GetSpace(guids.OfficeGuid, guids.SpaceGuid)).Value;
            var response = space.SpaceId;
            var responseBytes = Encoding.UTF8.GetBytes(response.ToString());
            if (space != null)
            {
                channel.BasicPublish(exchange: ReplyExchange,
                                     routingKey: ReplyBindingKeys[0],
                                     basicProperties: ReplyProperties,
                                     body: responseBytes);

                channel.BasicAck(deliveryTag: ea.DeliveryTag,
                                 multiple: false);
                Console.WriteLine("Reply is sent via " + ReplyBindingKeys[0] + " BindingKey");
            }
            if (space == null)
            {
                channel.BasicPublish(exchange: ReplyExchange,
                                    routingKey: ReplyBindingKeys[1],
                                    basicProperties: ReplyProperties,
                                    body: responseBytes);

                channel.BasicAck(deliveryTag: ea.DeliveryTag,
                                 multiple: false);
                Console.WriteLine("Reply is sent via " + ReplyBindingKeys[1] + " BindingKey");
            }
        }
        #endregion

        #region Constructor
        public SpaceServiceServer(
            [FromServices] IRabbitMQPersistentConnection persistentConnection,
            [FromServices] SpaceController spaceController)
        {
            this.persistentConnection = persistentConnection;
            this.spaceController = spaceController;
            CreateConsumerChannel(RequestQueueName);
            Console.WriteLine("SpaceService: created a queue called [" + RequestQueueName + "]");
        }
        #endregion

        public void CreateConsumerChannel(string queueName)
        {
            if (!persistentConnection.IsConnected)
            {
                persistentConnection.TryConnect();
            }

            #region Channel Settings
            var channel = persistentConnection.CreateModel();
            channel.ExchangeDeclare(RequestExchange, ExchangeType.Direct);
            channel.QueueDeclare(RequestQueueName, false, false, true);
            channel.QueueBind(RequestQueueName, RequestExchange, RequestBindingKey);
            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(queue: queueName,
                                 autoAck: false,
                                 consumer: consumer);
            #endregion

            consumer.Received += (model, ea) =>
            {
                MessageReceived(model, ea, channel);
            };




        }
    }
}
