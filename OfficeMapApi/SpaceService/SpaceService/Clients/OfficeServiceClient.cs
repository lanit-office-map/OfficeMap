using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SpaceService.Clients.Interfaces;
using Common.RabbitMQ.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using Common.RabbitMQ.Models;
using Common.Response;
using Microsoft.Extensions.Logging;

namespace SpaceService.Clients
{
  public class OfficeServiceClient : IOfficeServiceClient
  {
    #region private constants
    private const string RequestQueueName = "OfficeService_RequestQueue";
    private const string ResponseQueueName = "OfficeService_ReplyQueue";
    private const string RequestBindingKey = "OfficeRequest";
    private readonly StringCollection ResponseBindingKeys = new StringCollection()
    {
      "office_data", "office_error"
    };
    private const string RequestExchange = "requests";
    private const string ResponseExchange = "replies";
    #endregion

    #region private fields
    private readonly IModel channel;
    private IBasicProperties properties;
    private EventingBasicConsumer consumer;
    private string correlationId;

    private readonly BlockingCollection<Response<GetOfficeResponse>> Responses =
      new BlockingCollection<Response<GetOfficeResponse>>();

    private readonly ILogger<OfficeServiceClient> logger;
    #endregion


    #region private methods
    private Response<GetOfficeResponse> Message(GetOfficeRequest request)
    {
      var jsonRequest = JsonConvert.SerializeObject(request);
      var message = Encoding.UTF8.GetBytes(jsonRequest);
      properties = channel.CreateBasicProperties();
      correlationId = Guid.NewGuid().ToString();

      properties.CorrelationId = correlationId;
      properties.ReplyTo = ResponseQueueName;

      channel.BasicPublish(
        exchange: RequestExchange,
        routingKey: RequestBindingKey,
        basicProperties: properties,
        body: message);

      channel.BasicConsume(
        consumer: consumer,
        queue: ResponseQueueName,
        autoAck: true);

      var item = Responses.Take();
      return item;
    }

    private async Task MessageReceived(object model, BasicDeliverEventArgs ea, IModel channel)
    {
      var body = ea.Body;
      var response = Encoding.UTF8.GetString(body.ToArray());
      var feedback = JsonConvert.DeserializeObject<Response<GetOfficeResponse>>(response);
      if (ea.BasicProperties.CorrelationId == correlationId)
      {
        Responses.Add(feedback);
        logger.LogInformation("An Office is received");
      }
      else
      {
        logger.LogInformation(
          "Message with correlation id '{CorrelationId}' does not match the office client correlation id",
          ea.BasicProperties.CorrelationId);
      }
    }
    #endregion
    #region Constructor
    public OfficeServiceClient(
      [FromServices] IRabbitMQPersistentConnection rabbitMQPersistentConnection,
      [FromServices] ILogger<OfficeServiceClient> logger)
    {
      this.logger = logger;
      logger.LogInformation("OfficeServiceClient is created");

      channel = rabbitMQPersistentConnection.CreateModel();

      channel.ExchangeDeclare(RequestExchange, ExchangeType.Direct);
      channel.ExchangeDeclare(ResponseExchange, ExchangeType.Direct);

      channel.QueueDeclare(RequestQueueName, false, false, true);
      channel.QueueDeclare(ResponseQueueName, false, false, true);

      channel.QueueBind(ResponseQueueName, ResponseExchange, ResponseBindingKeys[0]);
      channel.QueueBind(ResponseQueueName, ResponseExchange, ResponseBindingKeys[1]);
      channel.QueueBind(RequestQueueName, RequestExchange, RequestBindingKey);

      consumer = new EventingBasicConsumer(channel);

      consumer.Received += async (model, ea) =>
      {
        await MessageReceived(model, ea, channel);
      };
    }
    #endregion

    public Task<Response<GetOfficeResponse>> GetOfficeAsync(GetOfficeRequest request)
    {
      logger.LogInformation("Request for Office is sent");
      var item = Message(request);
      return Task.FromResult(item);
    }
  }
}
