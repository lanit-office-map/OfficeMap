using Microsoft.AspNetCore.Mvc;
using System;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using Common.RabbitMQ.Interface;
using System.Collections.Specialized;
using Common.RabbitMQ.Models;
using Common.Response;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WorkplaceService.Clients.Interfaces;

namespace WorkplaceService.Clients
{
  public class UserServiceClient : IUserServiceClient
  {
    #region private constants
    private const string RequestQueueName = "UserService_Queue";
    private const string ResponseQueueName = "UserService_ReplyQueue";
    private const string RequestBindingKey = "UserRequest";

    private const string RequestExchange = "requests";
    private const string ResponseExchange = "replies";
    #endregion

    #region private fields
    private readonly IModel channel;
    private readonly IBasicProperties properties;
    private readonly EventingBasicConsumer consumer;
    private readonly StringCollection ResponseBindingKeys = new StringCollection()
        {
          "user_data", "user_error"
        };
    private readonly BlockingCollection<Response<GetUserResponse>> Responses = new BlockingCollection<Response<GetUserResponse>>();

    #endregion

    #region private methods
    private Response<GetUserResponse> Message(GetUserRequest request)
    {
      var message = JsonConvert.SerializeObject(request);
      var messageBytes = Encoding.UTF8.GetBytes(message);

      channel.BasicPublish(
        exchange: RequestExchange,
        routingKey: RequestBindingKey,
        basicProperties: properties,
        body: messageBytes);

      channel.BasicConsume(
        consumer: consumer,
        queue: ResponseQueueName,
        autoAck: true);
      var item = Responses.Take();
      return item;
    }
    #endregion

    #region public methods
    public UserServiceClient(
      [FromServices] IRabbitMQPersistentConnection rabbitMQPersistentConnection,
      [FromServices] ILogger<UserServiceClient> logger)
    {
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
        if (ea.RoutingKey == ResponseBindingKeys[0])
        {
          var body = ea.Body;
          var response = Encoding.UTF8.GetString(body.ToArray());
          var feedback =
            JsonConvert.DeserializeObject<Response<GetUserResponse>>(response);
          if (ea.BasicProperties.CorrelationId == correlationId)
          {
            Responses.Add(feedback);
            logger.LogInformation("User is received.");
          }
          else
          {
            logger.LogInformation(
              "Message with correlation id '{CorrelationId}' does not match the user client correlation id.",
              ea.BasicProperties.CorrelationId);
          }
        }
      };
    }

    public Task<Response<GetUserResponse>> GetUserAsync(GetUserRequest request)
    {
      Console.WriteLine("Request for Workplaces is sent");
      var item = Message(request);
      return Task.FromResult(item);
    }
    #endregion

  }
}