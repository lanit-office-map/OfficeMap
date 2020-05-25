using Common.RabbitMQ.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Common.RabbitMQ.Models;
using Common.Response;
using UserService.Models;
using UserService.Services.Interfaces;

namespace UserService.Servers
{
  public class UserServiceServer
  {
    #region private constants
    private const string ResponseExchange = "replies";
    private const string ResponseQueueName = "UserService_ReplyQueue";
    private const string RequestQueueName = "UserService_RequestQueue";
    private const string RequestExchange = "requests";
    private const string RequestBindingKey = "UserRequest";
    #endregion

    #region private fields
    private readonly IRabbitMQPersistentConnection persistentConnection;

    private readonly IUserService userService;

    private readonly ILogger<UserServiceServer> logger;
    private readonly StringCollection ResponseBindingKeys = new StringCollection()
        {
            "user_data", "user_error"
        };

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
      var userRequest = JsonConvert.DeserializeObject<GetUserRequest>(message);

      logger.LogInformation("User with guid '{userGuid}' is received", userRequest.UserGuid);

      var userResponse = await userService.GetAsync(userRequest.UserGuid);

      var response = JsonConvert.SerializeObject(
        autoMapper.Map<Response<GetUserResponse>>(userResponse));
      var responseBytes = Encoding.UTF8.GetBytes(response);
      var routingKey = ResponseBindingKeys[0];

      channel.BasicPublish(exchange: ResponseExchange,
        routingKey: routingKey,
        basicProperties: responseProperties,
        body: responseBytes);

      channel.BasicAck(deliveryTag: ea.DeliveryTag,
        multiple: false);
      logger.LogInformation("Response is sent via '{routingKey}' BindingKey", routingKey);
    }
    #endregion

    #region public methods
    #region Constructor
    public UserServiceServer(
      [FromServices] IRabbitMQPersistentConnection persistentConnection,
      [FromServices] ILogger<UserServiceServer> logger,
      [FromServices] IUserService userService,
      [FromServices] IMapper autoMapper)
    {
      this.logger = logger;
      this.persistentConnection = persistentConnection;
      this.userService = userService;
      this.autoMapper = autoMapper;

      CreateConsumerChannel(RequestQueueName);
      logger.LogInformation("UserService: created a queue called '{RequestQueueName}'", RequestQueueName);
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
    #endregion
  }
}
