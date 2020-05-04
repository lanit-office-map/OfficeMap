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
        private readonly IBasicProperties props;
        private readonly EventingBasicConsumer consumer;
        private readonly IRabbitMQPersistentConnection rabbitMQPersistentConnection;
        private readonly BlockingCollection<Office> respQueue = new BlockingCollection<Office>();
        public OfficeServiceClient([FromServices] IRabbitMQPersistentConnection rabbitMQPersistentConnection)
        {
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
            var item = Message(officeGuid);
            return Task.FromResult(item);
        }

        public Office Message(Guid officeguid)
        {
            var message = Encoding.UTF8.GetBytes(officeguid.ToString());
            channel.BasicPublish(
                exchange: "",
                routingKey: "officeguid_queue",
                basicProperties: props,
                body: message);

            channel.BasicConsume(
                consumer: consumer,
                queue: replyQueueName,
                autoAck: true);
            var item = respQueue.Take();
            return item;
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
