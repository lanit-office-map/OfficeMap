using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Specialized;
using Common.RabbitMQ.Interface;
using WorkplaceService.Services;
using WorkplaceService.Filters;
using System.Collections.Generic;
using WorkplaceService.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AutoMapper;
using Common.RabbitMQ.Models;
using Common.Response;
using Microsoft.Extensions.Logging;
using WorkplaceService.Services.Interfaces;

namespace WorkplaceService.Servers
{
  public class WorkplaceServiceServer
  {
    #region private constants
    private const string ResponseExchange = "replies";
    private const string ResponseQueueName = "WorkplaceService_ReplyQueue";

    private const string RequestQueueName = "WorkplaceService_RequestQueue";
    private const string RequestExchange = "requests";
    private const string RequestBindingKey = "WorkplacesRequest";
    #endregion

    #region private fields
    private readonly IRabbitMQPersistentConnection persistentConnection;
    private readonly IWorkplaceService workplaceService;
    private readonly StringCollection ResponseBindingKeys = new StringCollection()
        {
          "workplaces_data", "workplaces_error"
        };

    private readonly ILogger<WorkplaceServiceServer> logger;
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
      var workplacesRequest = JsonConvert.DeserializeObject<GetWorkplacesRequest>(message);
      logger.LogInformation("SpaceID received: " + message);

      var workplaces = await workplaceService.FindAllAsync(
        new WorkplaceFilter(workplacesRequest.SpaceId));

      var response = JsonConvert.SerializeObject(
        autoMapper.Map<Response<IEnumerable<GetWorkplaceResponse>>>(workplaces));
      var responseBytes = Encoding.UTF8.GetBytes(response);
      string routingKey = ResponseBindingKeys[0];

      channel.BasicPublish(exchange: ResponseExchange,
        routingKey: routingKey,
        basicProperties: responseProperties,
        body: responseBytes);

      channel.BasicAck(deliveryTag: ea.DeliveryTag,
        multiple: false);
      Console.WriteLine("Reply is sent via " + routingKey + " BindingKey");
    }
    #endregion

    #region Constructor
    public WorkplaceServiceServer(
        [FromServices] IRabbitMQPersistentConnection persistentConnection,
        [FromServices] IWorkplaceService workplaceService,
        [FromServices] ILogger<WorkplaceServiceServer> logger,
        [FromServices] IMapper autoMapper)
    {
      this.persistentConnection = persistentConnection;
      this.workplaceService = workplaceService;
      this.logger = logger;
      this.autoMapper = autoMapper;
      CreateConsumerChannel(RequestQueueName);
      logger.LogInformation("WorkplaceService: created a queue called [" + RequestQueueName + "]");
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
