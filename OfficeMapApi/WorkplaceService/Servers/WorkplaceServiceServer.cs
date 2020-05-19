using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Specialized;
using Common.RabbitMQ.Interface;
using WorkplaceService.Services;
using WorkplaceService.Filters;
using System.Collections.Generic;
using WorkplaceService.Models;
using System.Collections.ObjectModel;

namespace WorkplaceService.Servers
{
    public class WorkplaceServiceServer
    {
        #region private fields
        private readonly IRabbitMQPersistentConnection persistentConnection;
        private readonly IWorkplaceService workplaceService;
        #endregion
        private readonly StringCollection ReplyBindingKeys = new StringCollection()
        {
            "workplaces_data", "workplaces_error"
        };
        private const string ReplyExchange = "replies";
        private const string ReplyQueueName = "WorkplaceService_ReplyQueue";

        private const string RequestQueueName = "WorkplaceService_RequestQueue";
        private const string RequestExchange = "requests";
        private const string RequestBindingKey = "WorkplacesRequest";

        #region Private Methods
        private void MessageReceived(object? model, BasicDeliverEventArgs ea, IModel channel)
        {
            var InboundMessage = ea.Body;
            var InboundProperties = ea.BasicProperties;

            var ReplyProperties = channel.CreateBasicProperties();
            ReplyProperties.CorrelationId = InboundProperties.CorrelationId;
            ReplyProperties.ReplyTo = InboundProperties.ReplyTo;

            var message = Encoding.UTF8.GetString(InboundMessage.ToArray());
            int spaceId = int.Parse(message);
            Console.WriteLine("SpaceID received: " + message);

            ICollection<WorkplaceResponse> workplaces = new Collection<WorkplaceResponse>();
            WorkplaceResponse workplace1 = new WorkplaceResponse()
            {
                WorkplaceId = 1,
                Name = "mafaka"
            };
            WorkplaceResponse workplace2 = new WorkplaceResponse()
            {
                WorkplaceId = 2,
                Name = "mafaka2"
            };
            workplaces.Add(workplace1);
            workplaces.Add(workplace2);
            var response = JsonConvert.SerializeObject(workplaces);
            var responseBytes = Encoding.UTF8.GetBytes(response);

            if (workplaces != null)
            {
                channel.BasicPublish(exchange: ReplyExchange,
                                     routingKey: ReplyBindingKeys[0],
                                     basicProperties: ReplyProperties,
                                     body: responseBytes);

                channel.BasicAck(deliveryTag: ea.DeliveryTag,
                                 multiple: false);
                Console.WriteLine("Reply is sent via " + ReplyBindingKeys[0] + " BindingKey");
            }
            if (workplaces == null)
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
        public WorkplaceServiceServer(
            [FromServices] IRabbitMQPersistentConnection persistentConnection,
            [FromServices] IWorkplaceService workplaceService)
        {
            this.persistentConnection = persistentConnection;
            this.workplaceService = workplaceService;
            CreateConsumerChannel(RequestQueueName);
            Console.WriteLine("WorkplaceService: created a queue called [" + RequestQueueName + "]");
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
