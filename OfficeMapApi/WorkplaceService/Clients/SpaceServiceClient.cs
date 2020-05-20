using Microsoft.AspNetCore.Mvc;
using System;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using Common.RabbitMQ.Interface;
using WorkplaceService.Models.RabbitMQ;
using System.Collections.Specialized;
using Microsoft.Extensions.Logging;

namespace WorkplaceService.Clients
{
    public class SpaceServiceClient : ISpaceServiceClient
    {
        #region Private Fields
        private readonly IModel channel;
        private readonly IBasicProperties properties;
        private readonly EventingBasicConsumer consumer;
        private readonly ILogger<SpaceServiceClient> logger;
        private const string RequestQueueName = "SpaceService_Queue";
        private const string ReplyQueueName = "SpaceService_ReplyQueue";
        private const string RequestBindingKey = "SpaceRequest";
        private readonly BlockingCollection<Space> Replies = new BlockingCollection<Space>();
        private readonly StringCollection ReplyBindingKeys = new StringCollection()
        {
            "space_data", "space_error"
        };

        private const string RequestExchange = "requests";
        private const string ReplyExchange = "replies";
        private GetSpaceRequest Request = new GetSpaceRequest();

        #endregion

        #region Private Methods
        private Space Message(GetSpaceRequest Request)
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
        #endregion

        #region Constructor
        public SpaceServiceClient(
            [FromServices] IRabbitMQPersistentConnection rabbitMQPersistentConnection,
            [FromServices] ILogger<SpaceServiceClient> logger)
        {
            this.logger = logger;
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

            //TODO заставить сервис отправлять запросы и заменить заглушку на вызов метода GetSpaceIdAsync из контроллера
            
            Guid officeGuid = Guid.NewGuid();
            Guid spaceGuid = Guid.NewGuid();
      //      GetSpaceIdAsync(officeGuid, spaceGuid);


            consumer.Received += (model, ea) =>
            {
                if (ea.RoutingKey == "space_data")
                {
                    var body = ea.Body;
                    var response = Encoding.UTF8.GetString(body.ToArray());
                    var feedback = JsonConvert.DeserializeObject<Space>(response);
                    if (ea.BasicProperties.CorrelationId == correlationId)
                    {
                        Replies.Add(feedback);
                        logger.LogInformation("SpaceID received: " + body.ToString());
                    }
                }
                if (ea.RoutingKey == "space_error")
                {
                    Space feedback = null;
                    logger.LogInformation("404: No Space is found");
                    Replies.Add(feedback);
                }
            };
        }
        #endregion

        #region Public Methods
        public Task<Space> GetSpaceIdAsync(Guid officeGuid, Guid spaceGuid)
        {
            Request.OfficeGuid = officeGuid;
            Request.SpaceGuid = spaceGuid;
            logger.LogInformation("Request for the Space is sent");
            var item = Message(Request);
            return Task.FromResult(item);
        }
        #endregion
    }
}