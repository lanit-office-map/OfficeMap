using Microsoft.AspNetCore.Mvc;
using System;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Common.RabbitMQ.Interface;
using WorkplaceService.Models.RabbitMQ;
using System.Collections.Specialized;

namespace WorkplaceService.Clients
{
    public class SpaceServiceClient : ISpaceServiceClient
    {
        #region RabbitMQ Fields
        private readonly IModel channel;
        private readonly IBasicProperties properties;
        private readonly EventingBasicConsumer consumer;
        #endregion
        private const string RequestQueueName = "SpaceService_Queue";
        private const string ReplyQueueName = "SpaceService_ReplyQueue";
        private const string RequestBindingKey = "SpaceRequest";
        private readonly StringCollection ReplyBindingKeys = new StringCollection()
        {
            "space_data", "space_error"
        };

        private const string RequestExchange = "requests";
        private const string ReplyExchange = "replies";
        private GetSpaceRequest Request = new GetSpaceRequest();
        private readonly BlockingCollection<int> Replies = new BlockingCollection<int>();

        #region Constructor
        public SpaceServiceClient([FromServices] IRabbitMQPersistentConnection rabbitMQPersistentConnection)
        {
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

            Guid officeGuid = Guid.NewGuid();
            Guid spaceGuid = Guid.NewGuid();
            GetSpaceIdAsync(officeGuid, spaceGuid);


            consumer.Received += (model, ea) =>
            {
                if (ea.RoutingKey == "space_data")
                {
                    var body = ea.Body;
                    int feedback = int.Parse(body.ToString());
                    if (ea.BasicProperties.CorrelationId == correlationId)
                    {
                        Replies.Add(feedback);
                        Console.WriteLine("SpaceID received: " + body.ToString());
                    }
                }
                if (ea.RoutingKey == "space_error")
                {
                    // добавишь обработку 404
                    int feedback = 0;
                    Console.WriteLine("404: No Space is found");
                    Replies.Add(feedback);
                }
            };
        }
        #endregion
        public Task<int> GetSpaceIdAsync(Guid officeGuid, Guid spaceGuid)
        {
            Request.OfficeGuid = officeGuid;
            Request.SpaceGuid = spaceGuid;
            Console.WriteLine("Request for Workplaces is sent");
            var item = Message(Request);
            return Task.FromResult(item);
        }

        private int Message(GetSpaceRequest Request)
        {
            var message = JsonConvert.SerializeObject(Request);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(
                exchange: RequestExchange,
                routingKey: RequestBindingKey,
                basicProperties: properties,
                body: messageBytes);

            channel.BasicConsume(
                consumer: consumer,
                queue: ReplyQueueName,
                autoAck: true);
            var item = Replies.Take();
            return item;
        }
    }
}