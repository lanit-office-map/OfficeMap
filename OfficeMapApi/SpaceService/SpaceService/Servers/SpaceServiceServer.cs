using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SpaceService.Clients;
using SpaceService.Controllers;
using SpaceService.Filters;
using SpaceService.Models;
using SpaceService.Services.Interfaces;
using System;
using System.Collections.Specialized;
using System.Text;
using AutoMapper;
using Common.RabbitMQ.Interface;
using Common.RabbitMQ.Models;
using Common.Response;

namespace SpaceService.Servers
{
  public class SpaceServiceServer
  {
    #region private constants
    private const string ResponseExchange = "replies";
    private const string ReplyQueueName = "SpaceService_ReplyQueue";

    private const string RequestQueueName = "SpaceService_RequestQueue";
    private const string RequestExchange = "requests";
    private const string RequestBindingKey = "SpaceRequest";
    #endregion

    #region private fields
    private readonly IRabbitMQPersistentConnection persistentConnection;
    private readonly ISpacesService spacesService;
    private readonly ILogger<SpaceServiceServer> logger;

    private readonly StringCollection ResponseBindingKeys = new StringCollection()
    {
      "space_data", "space_error"
    };

    private readonly IMapper autoMapper;
    #endregion

    #region private methods
    private async void MessageReceived(object model, BasicDeliverEventArgs ea, IModel channel)
    {
      var inboundMessage = ea.Body;
      var inboundProperties = ea.BasicProperties;

      var responseProperties = channel.CreateBasicProperties();
      responseProperties.CorrelationId = inboundProperties.CorrelationId;
      responseProperties.ReplyTo = inboundProperties.ReplyTo;

      var message = Encoding.UTF8.GetString(inboundMessage.ToArray());
      GetSpaceRequest spaceRequest = JsonConvert.DeserializeObject<GetSpaceRequest>(message);
      logger.LogInformation("Space with guid '{spaceGuid}' is received: ", spaceRequest.SpaceGuid);

      var spaceResponse = await spacesService.GetAsync(spaceRequest.SpaceGuid);

      byte[] responseBytes =
        Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(
          autoMapper.Map<Response<GetSpaceResponse>>(spaceResponse)));
      string routingKey = ResponseBindingKeys[0];

      channel.BasicPublish(exchange: ResponseExchange,
        routingKey: routingKey,
        basicProperties: responseProperties,
        body: responseBytes);

      channel.BasicAck(deliveryTag: ea.DeliveryTag,
                         multiple: false);
      logger.LogInformation("Response is sent via " + routingKey + " BindingKey");

    }
    #endregion

    #region public methods
    #region Constructor
    public SpaceServiceServer(
      [FromServices] IRabbitMQPersistentConnection persistentConnection,
      [FromServices] ISpacesService spacesService,
      [FromServices] ILogger<SpaceServiceServer> logger,
      [FromServices] IMapper autoMapper)
    {
      this.spacesService = spacesService;
      this.persistentConnection = persistentConnection;
      this.logger = logger;
      this.autoMapper = autoMapper;
      CreateConsumerChannel(RequestQueueName);
      logger.LogInformation("SpaceService: created a queue called [" + RequestQueueName + "]");
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
    #endregion

  }
}
