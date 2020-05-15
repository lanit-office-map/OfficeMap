using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using WorkplaceService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Common.RabbitMQ.Interface;
using WorkplaceService.Controllers;
using WorkplaceService.Models.RabbitMQ;

namespace WorkplaceService.Servers
{
    public class WorkplaceServiceServer
    {
        private readonly IRabbitMQPersistentConnection persistentConnection;
        private readonly IWorkplaceService workplaceService;
        private readonly ILogger<WorkplaceServiceServer> logger;
        private readonly WorkplaceController workplaceController;
        private const string queue = "workplaceguid_queue";

        public WorkplaceServiceServer(
            [FromServices] IRabbitMQPersistentConnection persistentConnection,
            [FromServices] ILogger<WorkplaceServiceServer> logger,
            [FromServices] WorkplaceController workplaceController)
        {
            this.persistentConnection = persistentConnection;
            this.logger = logger;
            this.workplaceController = workplaceController;
            CreateConsumerChannel(queue);
            logger.LogInformation("WorkplaceService: created an " + queue);
        }

        public void CreateConsumerChannel(string queueName)
        {
            if (!persistentConnection.IsConnected)
            {
                persistentConnection.TryConnect();
            }

            var channel = persistentConnection.CreateModel();
            channel.QueueDeclare(queue: queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            channel.BasicQos(0, 1, false);
            var consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(queue: queueName,
                                 autoAck: false,
                                 consumer: consumer);

            consumer.Received += (model, ea) =>
            {
                ReceivedEvent(model, ea, channel);
            };
        }

        private async void ReceivedEvent(object? model, BasicDeliverEventArgs ea, IModel channel)
        {
            var body = ea.Body;
            var props = ea.BasicProperties;
            var replyProps = channel.CreateBasicProperties();
            replyProps.CorrelationId = props.CorrelationId;

            var message = Encoding.UTF8.GetString(body.ToArray());
            var request = JsonConvert.DeserializeObject<WorkplacesResponse>(message);
            logger.LogInformation("OfficeGUID: " + message);
            var workplaces = (await workplaceController.GetWorkplaces(request.OfficeGuid, request.SpaceGuid)).Value;

            var response = JsonConvert.SerializeObject(workplaces);
            var responseBytes = Encoding.UTF8.GetBytes(response);

            channel.BasicPublish(exchange: "",
                                 routingKey: props.ReplyTo,
                                 basicProperties: replyProps,
                                 body: responseBytes);

            channel.BasicAck(deliveryTag: ea.DeliveryTag,
                             multiple: false);
        }
    }
}
