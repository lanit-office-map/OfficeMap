using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SpaceService.Clients.Interfaces;
using SpaceService.Models;
using SpaceService.RabbitMQ.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceService.Clients
{
    public class WorkplaceServiceClient : IWorkplaceServiceClient
    {
        #region RabbitMQ Fields
        private readonly IModel channel;
        private readonly IBasicProperties properties;
        private readonly EventingBasicConsumer consumer;
        #endregion
        private const string RequestQueueName = "WorkplaceService_RequestQueue";
        private const string ReplyQueueName = "WorkplaceService_ReplyQueue";
        private const string RequestBindingKey = "WorkplacesRequest";
        private readonly StringCollection ReplyBindingKeys = new StringCollection()
        {
            "workplaces_data", "workplaces_error"
        };

        private const string RequestExchange = "requests";
        private const string ReplyExchange = "replies";
        private readonly BlockingCollection<IEnumerable<WorkplaceResponse>> Replies = new BlockingCollection<IEnumerable<WorkplaceResponse>>();

        #region Private Methods
        private IEnumerable<WorkplaceResponse> Message(int spaceId)
        {
            var message = Encoding.UTF8.GetBytes(spaceId.ToString());

            channel.BasicPublish(
                exchange: RequestExchange,
                routingKey: RequestBindingKey,
                basicProperties: properties,
                body: message);

            channel.BasicConsume(
                consumer: consumer,
                queue: ReplyQueueName,
                autoAck: true);
            var item = Replies.Take();
            return item;
        }
        #endregion

        #region Constructor
        public WorkplaceServiceClient([FromServices] IRabbitMQPersistentConnection rabbitMQPersistentConnection)
        {
            Console.WriteLine("WorkplaceServiceClient is created");
            channel = rabbitMQPersistentConnection.CreateModel();

            var correlationId = Guid.NewGuid().ToString();
            properties = channel.CreateBasicProperties();
            properties.CorrelationId = correlationId;
            properties.ReplyTo = ReplyQueueName;

            consumer = new EventingBasicConsumer(channel);

            channel.ExchangeDeclare(RequestExchange, ExchangeType.Direct);
            channel.ExchangeDeclare(ReplyExchange, ExchangeType.Direct);

            channel.QueueDeclare(RequestQueueName, false, false, true);
            channel.QueueDeclare(ReplyQueueName, false, false, true);

            channel.QueueBind(ReplyQueueName, ReplyExchange, ReplyBindingKeys[0]);
            channel.QueueBind(ReplyQueueName, ReplyExchange, ReplyBindingKeys[1]);
            channel.QueueBind(RequestQueueName, RequestExchange, RequestBindingKey);



            consumer.Received += (model, ea) =>
            {
                if (ea.RoutingKey == ReplyBindingKeys[0])
                {
                    var body = ea.Body;
                    var response = Encoding.UTF8.GetString(body.ToArray());
                    var feedback = JsonConvert.DeserializeObject<IEnumerable<WorkplaceResponse>>(response);
                    if (ea.BasicProperties.CorrelationId == correlationId)
                    {
                        Replies.Add(feedback);
                    }
                    Console.WriteLine("Workplaces are received");
                }
                if (ea.RoutingKey == ReplyBindingKeys[1])
                {
                    Console.WriteLine("404: No Workplaces found");
                }
            };
        }
        #endregion
        public Task<IEnumerable<WorkplaceResponse>> GetWorkplacesAsync(int spaceId)
        {
            Console.WriteLine("Request for Workplaces is sent");
            var item = Message(spaceId);
            return Task.FromResult(item);
        }

        
    }
}
