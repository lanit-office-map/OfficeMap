using Common.RabbitMQ.Interface;
using Common.Response;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using OfficeService.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using Microsoft.Extensions.Logging;

namespace OfficeService.Messaging.RabbitMQ
{
    public class OfficeServiceServer
    {
        #region private fields
        private readonly IRabbitMQPersistentConnection persistentConnection;
        private readonly IOfficeService officeService;
        private readonly ILogger<OfficeServiceServer> logger;
        private readonly StringCollection ReplyBindingKeys = new StringCollection()
        {
            "office_data", "office_error"
        };
        private const string ReplyExchange = "replies";
        private const string ReplyQueueName = "OfficeService_ReplyQueue";
        private const string RequestQueueName = "OfficeService_RequestQueue";
        private const string RequestExchange = "requests";
        private const string RequestBindingKey = "OfficeRequest";
        #endregion

        #region Private Methods
        private async void MessageReceived(object? model, BasicDeliverEventArgs ea, IModel channel)
        {
            var InboundMessage = ea.Body;
            var InboundProperties = ea.BasicProperties;

            var ReplyProperties = channel.CreateBasicProperties();
            ReplyProperties.CorrelationId = InboundProperties.CorrelationId;
            ReplyProperties.ReplyTo = InboundProperties.ReplyTo;

            var message = Encoding.UTF8.GetString(InboundMessage.ToArray());
            Guid officeguid = Guid.Parse(message);
            logger.LogInformation("OfficeGUID received: " + message);

            var office = (await officeService.GetAsync(officeguid)).Result;
            var response = JsonConvert.SerializeObject(office);
            var responseBytes = Encoding.UTF8.GetBytes(response);
            if (office != null)
            {
                channel.BasicPublish(exchange: ReplyExchange,
                                     routingKey: ReplyBindingKeys[0],
                                     basicProperties: ReplyProperties,
                                     body: responseBytes);

                channel.BasicAck(deliveryTag: ea.DeliveryTag,
                                 multiple: false);
                logger.LogInformation("Reply is sent via " + ReplyBindingKeys[0] + " BindingKey");
            }
            if (office == null)
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
        public OfficeServiceServer(
            [FromServices] IRabbitMQPersistentConnection persistentConnection,
            [FromServices] IOfficeService officeService,
            [FromServices] ILogger<OfficeServiceServer> logger)
        {
            this.logger = logger;
            this.persistentConnection = persistentConnection;
            this.officeService = officeService;
            CreateConsumerChannel(RequestQueueName);
            logger.LogInformation("OfficeService: created a queue called [" + RequestQueueName + "]");
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
