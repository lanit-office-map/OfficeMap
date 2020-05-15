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

namespace WorkplaceService.Clients
{
    public class UserServiceClient : IUserServiceClient
    {
        private readonly IModel channel;
        private readonly string replyQueueName;
        private readonly IBasicProperties props;
        private readonly EventingBasicConsumer consumer;
        private readonly IRabbitMQPersistentConnection rabbitMQPersistentConnection;
        private readonly BlockingCollection<Employee> respQueue = new BlockingCollection<Employee>();
        private readonly ILogger<UserServiceClient> logger;

        public UserServiceClient(
            [FromServices] IRabbitMQPersistentConnection rabbitMQPersistentConnection,
            [FromServices] ILogger<UserServiceClient> logger)
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
                var feedback = JsonConvert.DeserializeObject<Employee>(response);
                if (ea.BasicProperties.CorrelationId == correlationId)
                {
                    respQueue.Add(feedback);
                }
            };
        }

        public Task<Employee> GetUserIdAsync(Guid employeeGuid)
        {
            var employeeRequest = new GetEmployeeRequest { EmployeeGuid = employeeGuid };
            logger.LogInformation("HTTP-request is sent");
            var item = Message(employeeRequest);
            return Task.FromResult(item);
        }

        private Employee Message(GetEmployeeRequest employeeRequest)
        {
            var employeeRequestJson = JsonConvert.SerializeObject(employeeRequest);
            var message = Encoding.UTF8.GetBytes(employeeRequestJson);
            channel.BasicPublish(
                exchange: "",
                routingKey: "userService_queue",
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