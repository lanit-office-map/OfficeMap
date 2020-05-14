using Microsoft.AspNetCore.Mvc;
using System;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using WorkplaceService.Models;
using Microsoft.Extensions.Logging;
using Common.RabbitMQ.Interface;

namespace WorkplaceService.Clients
{
    public class SpaceServiceClient : ISpaceServiceClient
    {
        private readonly IModel channel;
        private readonly string replyQueueName;
        private readonly IBasicProperties props;
        private readonly EventingBasicConsumer consumer;
        private readonly IRabbitMQPersistentConnection rabbitMQPersistentConnection;
        private readonly BlockingCollection<Space> respQueue = new BlockingCollection<Space>();
        private readonly ILogger<SpaceServiceClient> logger;

        public SpaceServiceClient(
            [FromServices] IRabbitMQPersistentConnection rabbitMQPersistentConnection,
            [FromServices] ILogger<SpaceServiceClient> logger)
        {
            this.logger = logger;

            this.rabbitMQPersistentConnection = rabbitMQPersistentConnection;
            channel = rabbitMQPersistentConnection.CreateModel();
            replyQueueName = channel.QueueDeclare().QueueName;
            consumer = new EventingBasicConsumer(channel);

            props = channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;
            props.ReplyTo = replyQueueName;

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var response = Encoding.UTF8.GetString(body.ToArray());
                var feedback = JsonConvert.DeserializeObject<Space>(response);
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    respQueue.Add(feedback);
                }
            };
        }

        public Task<Space> GetSpaceGuidsAsync(Guid officeGuid, Guid spaceGuid)
        {
            var spaceRequest = new GetSpaceRequest { OfficeGuid = officeGuid, SpaceGuid = spaceGuid };
            logger.LogInformation("HTTP-request is sent");
            var item = Message(spaceRequest);
            return Task.FromResult(item);
        }

        private Space Message(GetSpaceRequest spaceRequest)
        {
            var spaceRequestJson = JsonConvert.SerializeObject(spaceRequest);
            var message = Encoding.UTF8.GetBytes(spaceRequestJson);
            channel.BasicPublish(
                exchange: "",
                routingKey: "spaceService_queue",
                basicProperties: props,
                body: message);

            channel.BasicConsume(
                consumer: consumer,
                queue: replyQueueName,
                autoAck: true);
            var item = respQueue.Take();
            return item;
        }
    }
}