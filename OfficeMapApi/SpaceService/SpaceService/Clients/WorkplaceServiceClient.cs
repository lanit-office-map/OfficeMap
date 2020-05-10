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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceService.Clients
{
    public class WorkplaceServiceClient : IWorkplaceServiceClient
    {
        private readonly IModel channel;
        private readonly string replyQueueName;
        private readonly IBasicProperties props;
        private readonly EventingBasicConsumer consumer;
        private readonly IRabbitMQPersistentConnection rabbitMQPersistentConnection;
        private readonly BlockingCollection<IEnumerable<WorkplaceResponse>> respQueue = new BlockingCollection<IEnumerable<WorkplaceResponse>>();
        public WorkplaceServiceClient([FromServices] IRabbitMQPersistentConnection rabbitMQPersistentConnection)
        {
            this.rabbitMQPersistentConnection = rabbitMQPersistentConnection;
            channel = rabbitMQPersistentConnection.CreateModel();
            replyQueueName = channel.QueueDeclare().QueueName;
            consumer = new EventingBasicConsumer(channel);

            props = channel.CreateBasicProperties();
            var correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;
            props.ReplyTo = replyQueueName;
            
            Console.WriteLine("SpaceService is connected to RabbitMQ ready to get feedback from WorkplaceService");

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var response = Encoding.UTF8.GetString(body.ToArray());
                var feedback = JsonConvert.DeserializeObject<IEnumerable<WorkplaceResponse>>(response);
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    respQueue.Add(feedback);
                }
            };
        }
        public Task<IEnumerable<WorkplaceResponse>> GetWorkplacesAsync(Guid spaceGuid)
        {
            Console.WriteLine("HTTP-request is sent");
            var item = Message(spaceGuid);
            return Task.FromResult(item);
        }

        public IEnumerable<WorkplaceResponse> Message(Guid spaceguid)
        {
            var message = Encoding.UTF8.GetBytes(spaceguid.ToString());
            channel.BasicPublish(
                exchange: "",
                routingKey: "spaceguid_queue",
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
