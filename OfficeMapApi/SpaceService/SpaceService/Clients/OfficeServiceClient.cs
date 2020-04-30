using Microsoft.AspNetCore.Mvc;
using SpaceService.Models;
using SpaceService.Services.Interfaces;
using System;
using SpaceService.RabbitMQ.Interface;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using Newtonsoft.Json;

namespace SpaceService.Clients
{
    public class OfficeServiceClient : IOfficeServiceClient
    {
        private readonly IModel channel;
        private readonly string replyQueueName;
        private readonly EventingBasicConsumer consumer;
        private readonly BlockingCollection<Office> respQueue = new BlockingCollection<Office>();
        private readonly IBasicProperties props;
        private readonly IRabbitMQPersistentConnection rabbitMQPersistentConnection;

        private readonly ISpacesService spacesService;
        public OfficeServiceClient(
            [FromServices] ISpacesService spacesService,
            [FromServices] IRabbitMQPersistentConnection rabbitMQPersistentConnection
        )
        {
            this.rabbitMQPersistentConnection = rabbitMQPersistentConnection;
            this.spacesService = spacesService;
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
                var feedback = JsonConvert.DeserializeObject<Office>(response);
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    respQueue.Add(feedback);
                }
            };
        }
            public Task<Office> GetOfficeAsync(Guid officeGuid)
        {
            Console.WriteLine("HTTP-request is sent");
            var messageBytes = Encoding.UTF8.GetBytes(officeGuid.ToString());
                channel.BasicPublish(
                    exchange: "",
                    routingKey: "officeguid_queue",
                    basicProperties: props,
                    body: messageBytes);

                channel.BasicConsume(
                    consumer: consumer,
                    queue: replyQueueName,
                    autoAck: true);
            var item = respQueue.Take();
            Console.WriteLine("SpaceService received answer from OfficeService");
            return Task.FromResult(item);
        }

        public void Close()
        {
            rabbitMQPersistentConnection.Close();
        }

        public void Dispose()
        {
            rabbitMQPersistentConnection.Dispose();
        }
    }
}
