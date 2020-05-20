using Microsoft.AspNetCore.Mvc;
using System;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using Common.RabbitMQ.Interface;
using WorkplaceService.Models.RabbitMQ;
using System.Collections.Specialized;
using Newtonsoft.Json;

namespace WorkplaceService.Clients
{
    public class UserServiceClient : IUserServiceClient
    {
        #region RabbitMQ Fields
        private readonly IModel channel;
        private readonly IBasicProperties properties;
        private readonly EventingBasicConsumer consumer;
        #endregion
        private const string RequestQueueName = "UserService_Queue";
        private const string ReplyQueueName = "UserService_ReplyQueue";
        private const string RequestBindingKey = "UserRequest";
        private readonly StringCollection ReplyBindingKeys = new StringCollection()
        {
            "user_data", "user_error"
        };

        private const string RequestExchange = "requests";
        private const string ReplyExchange = "replies";
        private GetEmployeeRequest Request = new GetEmployeeRequest();
        private readonly BlockingCollection<Employee> Replies = new BlockingCollection<Employee>();

        #region Constructor
        public UserServiceClient([FromServices] IRabbitMQPersistentConnection rabbitMQPersistentConnection)
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

            consumer.Received += (model, ea) =>
            {
                if (ea.RoutingKey == "user_data")
                {
                    var body = ea.Body;
                    var response = Encoding.UTF8.GetString(body.ToArray());
                    var feedback = JsonConvert.DeserializeObject<Employee>(response);
                    if (ea.BasicProperties.CorrelationId == correlationId)
                    {
                        Replies.Add(feedback);
                        Console.WriteLine("UserID received: " + body.ToString());
                    }
                }
                if (ea.RoutingKey == "user_error")
                {
                    // TODO Implement ErrorHandler
                    Employee feedback = null;
                    Console.WriteLine("404: No User is found");
                    Replies.Add(feedback);
                }
            };
        }
        #endregion
        public Task<Employee> GetUserIdAsync(Guid UserGuid)
        {
            Request.EmployeeGuid = UserGuid;
            Console.WriteLine("Request for Workplaces is sent");
            var item = Message(Request);
            return Task.FromResult(item);
        }

        private Employee Message(GetEmployeeRequest Request)
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