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
    public class OfficeServiceClient : IOfficeServiceClient
    {
        #region RabbitMQ Fields
        private readonly IModel channel;
        private readonly IBasicProperties properties;
        private readonly EventingBasicConsumer consumer;
        #endregion
        #region Constants
        private const string RequestQueueName = "OfficeService_RequestQueue";
        private const string ReplyQueueName = "OfficeService_ReplyQueue";
        private const string RequestBindingKey = "OfficeRequest";
        private readonly StringCollection ReplyBindingKeys = new StringCollection()
        {
            "office_data", "office_error"
        };
        private const string RequestExchange = "requests";
        private const string ReplyExchange = "replies";
        #endregion
        private readonly BlockingCollection<Office> Replies = new BlockingCollection<Office>();

        #region Private Methods
        private Office Message(Guid officeGuid)
        {
            var message = Encoding.UTF8.GetBytes(officeGuid.ToString());

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
        public OfficeServiceClient([FromServices] IRabbitMQPersistentConnection rabbitMQPersistentConnection)
        {
            Console.WriteLine("OfficeServiceClient is created");
            
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
                    var feedback = JsonConvert.DeserializeObject<Office>(response);
                    if (ea.BasicProperties.CorrelationId == correlationId)
                    {
                        Replies.Add(feedback);
                    }
                    Console.WriteLine("Office is received. OfficeId:" + feedback.OfficeId.ToString());
                }
                if (ea.RoutingKey == ReplyBindingKeys[1])
                {
                    Console.WriteLine("404: No Office is Found");
                }
            };
        }
        #endregion
    
        public Task<Office> GetOfficeAsync(Guid officeGuid)
        {
            Console.WriteLine("Request for Office is sent");
            var item = Message(officeGuid);
            return Task.FromResult(item);
        }
    }
}
