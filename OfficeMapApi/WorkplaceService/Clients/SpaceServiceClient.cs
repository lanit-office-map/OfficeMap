using Microsoft.AspNetCore.Mvc;
using System;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using WorkplaceService.Messaging.RabbitMQ.Interface;
using System.Linq;

namespace WorkplaceService.Clients
{
    public class SpaceServiceClient : ISpaceServiceClient
    {
        private readonly IModel channel;
        private readonly string replyQueueName;
        private readonly IBasicProperties props;
        private readonly EventingBasicConsumer consumer;
        private readonly IRabbitMQPersistentConnection rabbitMQPersistentConnection;
        private readonly BlockingCollection<IQueryable<Guid>> respQueue = new BlockingCollection<IQueryable<Guid>>();
        public SpaceServiceClient([FromServices] IRabbitMQPersistentConnection rabbitMQPersistentConnection)
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
                var feedback = JsonConvert.DeserializeObject<IQueryable<Guid>>(response);
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    respQueue.Add(feedback);
                }
            };
        }
        public Task<IQueryable<Guid>> GetSpaceGuidsAsync(Guid officeGuid)
        {
            Console.WriteLine("HTTP-request is sent");
            var item = Message(officeGuid);
            return Task.FromResult(item);
        }

        public IQueryable<Guid> Message(Guid officeguid)
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