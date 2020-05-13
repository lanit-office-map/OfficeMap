using Microsoft.AspNetCore.Mvc;
using SpaceService.Models;
using System;
using SpaceService.RabbitMQ.Interface;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using SpaceService.Clients.Interfaces;
using System.Collections.Generic;

namespace SpaceService.Clients
{
    public class Producer : IOfficeServiceClient
    {
        private readonly BlockingCollection<int> test_queue = new BlockingCollection<int>();
        
        private readonly IModel channel;
        private readonly string officeservice_replyQueueName;
        private readonly string workplaceservice_replyQueueName;
        private readonly IBasicProperties officeservice_props;
        private readonly IBasicProperties workplaceservice_props;
        private readonly EventingBasicConsumer consumer;
        private readonly IRabbitMQPersistentConnection rabbitMQPersistentConnection;
        private readonly BlockingCollection<Office> officeservice_callback_queue = new BlockingCollection<Office>();
        private readonly BlockingCollection<IEnumerable<WorkplaceResponse>> workplaceservice_callback_queue = new BlockingCollection<IEnumerable<WorkplaceResponse>>();
        public Producer([FromServices] IRabbitMQPersistentConnection rabbitMQPersistentConnection)
        {
            this.rabbitMQPersistentConnection = rabbitMQPersistentConnection;
            channel = rabbitMQPersistentConnection.CreateModel();
            officeservice_replyQueueName = channel.QueueDeclare().QueueName;
            workplaceservice_replyQueueName = channel.QueueDeclare().QueueName;
            consumer = new EventingBasicConsumer(channel);

            channel.ExchangeDeclare("IDs", ExchangeType.Direct);

            channel.QueueBind("officeservice_queue", "IDs", "officeguid");
            channel.QueueBind("workplaceservice_queue", "IDs", "spaceid");

                
            workplaceservice_props = channel.CreateBasicProperties();
            officeservice_props = channel.CreateBasicProperties();

            officeservice_props.CorrelationId = Guid.NewGuid().ToString();
            workplaceservice_props.CorrelationId = Guid.NewGuid().ToString();

            officeservice_props.ReplyTo = officeservice_replyQueueName;
            workplaceservice_props.ReplyTo = workplaceservice_replyQueueName;

            Console.WriteLine($"SpaceService is ready to get messages from WS: {workplaceservice_props.ReplyTo} /n OS: {officeservice_props.ReplyTo} ");


            consumer.Received += (model, ea) =>
            {
                if (ea.RoutingKey == officeservice_props.ReplyTo)
                {
                    var body = ea.Body;
                    var response = Encoding.UTF8.GetString(body.ToArray());
                    var feedback = JsonConvert.DeserializeObject<Office>(response);
                    if (ea.BasicProperties.CorrelationId == officeservice_props.CorrelationId)
                    {
                        officeservice_callback_queue.Add(feedback);
                    }
                    Console.WriteLine("Office is received");
                }
                if (ea.RoutingKey == workplaceservice_props.ReplyTo)
                {
                    var body = ea.Body;
                    var response = Encoding.UTF8.GetString(body.ToArray());
                    Console.WriteLine("Answer from WS is received. Implementation of models remains on the side of WS");
                    var feedback = Convert.ToInt32(response);
                    test_queue.Add(feedback);
                    /*
                    var feedback = JsonConvert.DeserializeObject<IEnumerable<WorkplaceResponse>>(response);
                    if (ea.BasicProperties.CorrelationId == workplaceservice_props.CorrelationId)
                    {
                        workplaceservice_callback_queue.Add(feedback);
                    }
                    Console.WriteLine("Workplaces are received");
                    */
                }
            };
        }
            public Task<Office> GetOfficeAsync(Guid officeGuid)
        {
            Console.WriteLine("HTTP-request for office is sent");
            var item = Message(officeGuid);
            return Task.FromResult(item);
        }

            public Task<int> GetWorkplacesAsync(int spaceid)
        {
            Console.WriteLine("HTTP-request for workplaces is sent");
            var item = Message(spaceid);
            return Task.FromResult(item);
        }

        public Office Message(Guid officeguid)
        {
            var message = Encoding.UTF8.GetBytes(officeguid.ToString());
            channel.BasicPublish(
                exchange: "IDs",
                routingKey: "officeguid",
                basicProperties: officeservice_props,
                body: message);
            channel.BasicConsume(
                consumer: consumer,
                queue: officeservice_replyQueueName,
                autoAck: true);
            var item = officeservice_callback_queue.Take();
            return item;
        }

        public int Message(int spaceid)
        {
            var message = Encoding.UTF8.GetBytes(spaceid.ToString());
            channel.BasicPublish(
                exchange: "IDs",
                routingKey: "spaceid",
                basicProperties: workplaceservice_props,
                body: message);

            channel.BasicConsume(
                consumer: consumer,
                queue: workplaceservice_replyQueueName,
                autoAck: true);
            var item = test_queue.Take();
            return item;
        }

        public void Close()
        {
            rabbitMQPersistentConnection.Close();
        }

        public void Dispose()
        {
            rabbitMQPersistentConnection.Dispose();
        }
    }
}
