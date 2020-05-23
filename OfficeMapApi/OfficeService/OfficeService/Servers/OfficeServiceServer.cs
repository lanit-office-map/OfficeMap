using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using Common.RabbitMQ.Interface;
using OfficeService.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System.Threading;
using AutoMapper;
using Common.RabbitMQ.Models;
using Common.Response;

namespace OfficeService.Servers
{
  public class OfficeServiceServer
  {
    #region private constants
    private const string queue = "officeguid_queue";
    #endregion

    #region private fields
    private readonly IRabbitMQPersistentConnection persistentConnection;
    private readonly IOfficeService officeService;
    private readonly ILogger<OfficeServiceServer> logger;
    private readonly IMapper autoMapper;
    #endregion

    #region private methods
    private async Task ReceivedEvent(
      object model,
      BasicDeliverEventArgs ea,
      IModel channel)
    {
      var body = ea.Body;
      var props = ea.BasicProperties;
      var replyProps = channel.CreateBasicProperties();
      replyProps.CorrelationId = props.CorrelationId;

      var message = Encoding.UTF8.GetString(body.ToArray());
      GetOfficeRequest officeRequest =
        JsonConvert.DeserializeObject<GetOfficeRequest>(message);
      logger.LogInformation("OfficeGUID: " + message);
      var officeResponse =
        await officeService.GetAsync(officeRequest.OfficeGuid);

      var response = JsonConvert.SerializeObject(
        autoMapper.Map<Response<GetOfficeResponse>>(officeResponse));
      var responseBytes = Encoding.UTF8.GetBytes(response);

      channel.BasicPublish(
        exchange: "",
        routingKey: props.ReplyTo,
        basicProperties: replyProps,
        body: responseBytes);

      channel.BasicAck(
        deliveryTag: ea.DeliveryTag,
        multiple: false);
    }

    #endregion

    #region public methods
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
      CreateConsumerChannel(queue);
      logger.LogInformation("OfficeService: created an " + queue);
    }

    public void CreateConsumerChannel(string queueName)
    {
      if (!persistentConnection.IsConnected)
      {
        persistentConnection.TryConnect();
      }

      var channel = persistentConnection.CreateModel();
      channel.QueueDeclare(
        queue: queueName,
        durable: false,
        exclusive: false,
        autoDelete: false,
        arguments: null);
      channel.BasicQos(0, 1, false);
      var consumer = new EventingBasicConsumer(channel);
      channel.BasicConsume(
        queue: queueName,
        autoAck: false,
        consumer: consumer);

      consumer.Received += async (model, ea) =>
      {
        await ReceivedEvent(model, ea, channel);
      };
    }
    #endregion
  }
}
