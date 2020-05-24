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
using AutoMapper;
using System.Threading.Tasks;
using Common.RabbitMQ.Models;

namespace OfficeService.Servers
{
    public class OfficeServiceServer
    {
        #region private constants
        private const string ResponseExchange = "replies";
        private const string ResponseQueueName = "OfficeService_ReplyQueue";

        private const string RequestQueueName = "OfficeService_RequestQueue";
        private const string RequestExchange = "requests";
        private const string RequestBindingKey = "OfficeRequest";
        #endregion

        #region private fields
        private readonly IRabbitMQPersistentConnection persistentConnection;
        private readonly IOfficeService officeService;
        private readonly StringCollection ResponseBindingKeys = new StringCollection()
        {
          "office_data", "office_error"
        };

        private readonly ILogger<OfficeServiceServer> logger;
        private readonly IMapper autoMapper;
        #endregion

        #region private methods
        private async Task MessageReceived(object model, BasicDeliverEventArgs ea, IModel channel)
        {
            var inboundMessage = ea.Body;
            var inboundProperties = ea.BasicProperties;

            var responseProperties = channel.CreateBasicProperties();
            responseProperties.CorrelationId = inboundProperties.CorrelationId;
            responseProperties.ReplyTo = inboundProperties.ReplyTo;

            var message = Encoding.UTF8.GetString(inboundMessage.ToArray());
            var officeRequest = JsonConvert.DeserializeObject<GetOfficeRequest>(message);
            logger.LogInformation($"OfficeGUID received: [{officeRequest.OfficeGuid}]");

            var office = await officeService.GetAsync(officeRequest.OfficeGuid);
            string routingKey;
            if (office.Error == null)
            {
                routingKey = ResponseBindingKeys[0];
            }
            else
            {
                routingKey = ResponseBindingKeys[1];
            }

            var response = JsonConvert.SerializeObject(
              autoMapper.Map<Response<GetOfficeResponse>>(office));
            var responseBytes = Encoding.UTF8.GetBytes(response);

            channel.BasicPublish(
              exchange: ResponseExchange,
              routingKey: routingKey,
              basicProperties: responseProperties,
              body: responseBytes);

            channel.BasicAck(
              deliveryTag: ea.DeliveryTag,
              multiple: false);
            logger.LogInformation($"Reply is sent via {routingKey} BindingKey");
        }
        #endregion

        #region Constructor
        public OfficeServiceServer(
        [FromServices] IRabbitMQPersistentConnection persistentConnection,
        [FromServices] IOfficeService officeService,
        [FromServices] ILogger<OfficeServiceServer> logger,
        [FromServices] IMapper autoMapper)
        {
            this.persistentConnection = persistentConnection;
            this.officeService = officeService;
            this.logger = logger;
            this.autoMapper = autoMapper;
            CreateConsumerChannel(RequestQueueName);
            logger.LogInformation($"OfficeService: created a queue called [{RequestQueueName}]");
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

            consumer.Received += async (model, ea) =>
            {
                await MessageReceived(model, ea, channel);
            };
        }
    }
}
