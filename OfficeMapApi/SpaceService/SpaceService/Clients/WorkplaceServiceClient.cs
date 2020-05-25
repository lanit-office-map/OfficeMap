using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SpaceService.Clients.Interfaces;
using SpaceService.Models;
using Common.RabbitMQ.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.RabbitMQ.Models;
using Common.Response;
using Microsoft.Extensions.Logging;

namespace SpaceService.Clients
{
  public class WorkplaceServiceClient : IWorkplaceServiceClient
  {
    #region private constants
    private const string RequestQueueName = "WorkplaceService_RequestQueue";
    private const string ResponseQueueName = "WorkplaceService_ReplyQueue";
    private const string RequestBindingKey = "WorkplacesRequest";
    private readonly StringCollection ResponseBindingKeys = new StringCollection()
    {
      "workplaces_data", "workplaces_error"
    };

    private const string RequestExchange = "requests";
    private const string ResponseExchange = "replies";
    #endregion

    #region private fields
    private readonly IModel channel;
    private readonly IBasicProperties properties;
    private readonly EventingBasicConsumer consumer;

    private readonly BlockingCollection<Response<IEnumerable<GetWorkplaceResponse>>> Responses =
      new BlockingCollection<Response<IEnumerable<GetWorkplaceResponse>>>();

    private readonly ILogger<WorkplaceServiceClient> logger;
    #endregion


    #region private methods
    private Response<IEnumerable<GetWorkplaceResponse>> Message(GetWorkplacesRequest request)
    {
      var jsonRequest = JsonConvert.SerializeObject(request);
      var message = Encoding.UTF8.GetBytes(jsonRequest);

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
    #endregion

    #region Constructor
    public WorkplaceServiceClient(
      [FromServices] IRabbitMQPersistentConnection rabbitMQPersistentConnection,
      [FromServices] ILogger<WorkplaceServiceClient> logger)
    {
      this.logger = logger;
      logger.LogInformation("WorkplaceServiceClient is created");
      channel = rabbitMQPersistentConnection.CreateModel();

      var correlationId = Guid.NewGuid().ToString();
      properties = channel.CreateBasicProperties();
      properties.CorrelationId = correlationId;
      properties.ReplyTo = ResponseQueueName;

      consumer = new EventingBasicConsumer(channel);

      channel.ExchangeDeclare(RequestExchange, ExchangeType.Direct);
      channel.ExchangeDeclare(ResponseExchange, ExchangeType.Direct);

      channel.QueueDeclare(RequestQueueName, false, false, true);
      channel.QueueDeclare(ResponseQueueName, false, false, true);

      channel.QueueBind(ResponseQueueName, ResponseExchange, ResponseBindingKeys[0]);
      channel.QueueBind(ResponseQueueName, ResponseExchange, ResponseBindingKeys[1]);
      channel.QueueBind(RequestQueueName, RequestExchange, RequestBindingKey);



      consumer.Received += (model, ea) =>
      {
        var body = ea.Body;
        var response = Encoding.UTF8.GetString(body.ToArray());
        var feedback = JsonConvert.DeserializeObject<Response<IEnumerable<GetWorkplaceResponse>>>(response);
        if (ea.BasicProperties.CorrelationId == correlationId)
        {
          Responses.Add(feedback);
          logger.LogInformation("Workplaces are received");
        }
        else
        {
          logger.LogInformation(
            "Message with correlation id '{CorrelationId}' does not match the workplace client correlation id",
            ea.BasicProperties.CorrelationId);
        }

      };
    }
    #endregion
    public Task<Response<IEnumerable<GetWorkplaceResponse>>> GetWorkplacesAsync(GetWorkplacesRequest request)
    {
      logger.LogInformation("Request for Workplaces is sent");
      var item = Message(request);
      return Task.FromResult(item);
    }
  }
}
