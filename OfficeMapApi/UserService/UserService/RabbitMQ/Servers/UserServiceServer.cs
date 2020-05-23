using Common.RabbitMQ.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Specialized;
using System.Text;
using UserService.Models.RabbitMQ_Models;

namespace UserService.RabbitMQ.Servers
{
    public class UserServiceServer
    {
        #region private fields
        private readonly IRabbitMQPersistentConnection persistentConnection;
        // добавить рабочий сервис для получения данных
        // private readonly IUserService userService;
        #endregion
        private readonly ILogger<UserServiceServer> logger;
        private readonly StringCollection ReplyBindingKeys = new StringCollection()
        {
            "user_data", "user_error"
        };
        private const string ReplyExchange = "replies";
        private const string ReplyQueueName = "UserService_ReplyQueue";
        private const string RequestQueueName = "UserService_RequestQueue";
        private const string RequestExchange = "requests";
        private const string RequestBindingKey = "UserRequest";

        #region Private Methods
        private void MessageReceived(object? model, BasicDeliverEventArgs ea, IModel channel)
        {
            var InboundMessage = ea.Body;
            var InboundProperties = ea.BasicProperties;

            var ReplyProperties = channel.CreateBasicProperties();
            ReplyProperties.CorrelationId = InboundProperties.CorrelationId;
            ReplyProperties.ReplyTo = InboundProperties.ReplyTo;

            var message = Encoding.UTF8.GetString(InboundMessage.ToArray());
            var EmployeeMessage = JsonConvert.DeserializeObject<Employee>(message);
            if (EmployeeMessage.EmployeeId != 0)
            {
                logger.LogInformation($"EmployeeID received: {EmployeeMessage.EmployeeId}");
            }
            if (EmployeeMessage.EmployeeGuid != null)
            {
                logger.LogInformation($"EmployeeGUID received: {EmployeeMessage.EmployeeGuid}");
            }
            // Заглушка 
            // TODO заменить на получение модели Employee из БД
            // e.g.
            // var Employee = await userService.GetEmployeeAsync(employee).Result;
            
            Employee employee = new Employee()
            {
                EmployeeId = 1
            };
            var response = JsonConvert.SerializeObject(EmployeeMessage);
            var responseBytes = Encoding.UTF8.GetBytes(response);

            if (EmployeeMessage != null)
            {
                channel.BasicPublish(exchange: ReplyExchange,
                                     routingKey: ReplyBindingKeys[0],
                                     basicProperties: ReplyProperties,
                                     body: responseBytes);

                channel.BasicAck(deliveryTag: ea.DeliveryTag,
                                 multiple: false);
                logger.LogInformation("Reply is sent via " + ReplyBindingKeys[0] + " BindingKey");
            }
            if (EmployeeMessage == null)
            {
                channel.BasicPublish(exchange: ReplyExchange,
                                    routingKey: ReplyBindingKeys[1],
                                    basicProperties: ReplyProperties,
                                    body: responseBytes);

                channel.BasicAck(deliveryTag: ea.DeliveryTag,
                                 multiple: false);
                logger.LogInformation("Reply is sent via " + ReplyBindingKeys[1] + " BindingKey");
            }
        }
        #endregion
        
        #region Constructor
        // TODO добавить [FromServices] рабочий сервис для получения данных и присвоить его полю класса
        public UserServiceServer(
            [FromServices] IRabbitMQPersistentConnection persistentConnection,
            [FromServices] ILogger<UserServiceServer> logger)
           // [FromServices] IWorkplaceService workplaceService)
        {
            this.logger = logger;
            this.persistentConnection = persistentConnection;
           // this.workplaceService = workplaceService;
            CreateConsumerChannel(RequestQueueName);
            logger.LogInformation("UserService: created a queue called [" + RequestQueueName + "]");
        }
        #endregion

        public void CreateConsumerChannel(string queueName)
        {
            if (!persistentConnection.IsConnected)
            {
                persistentConnection.TryConnect();
            }

            #region Channel Settings
            var channel = persistentConnection.CreateModel();
            channel.ExchangeDeclare(RequestExchange, ExchangeType.Direct);
            channel.QueueDeclare(RequestQueueName, false, false, true);
            channel.QueueBind(RequestQueueName, RequestExchange, RequestBindingKey);
            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(queue: queueName,
                                 autoAck: false,
                                 consumer: consumer);
            #endregion

            consumer.Received += (model, ea) =>
            {
                MessageReceived(model, ea, channel);
            };

        }
    }
}
