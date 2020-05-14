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
    public class ServicesClient : IOfficeServiceClient
    {

        #region Queue Names
        private const string officeservice_replyQueueName = "office_feedback";
        private const string workplaceservice_replyQueueName = "workplaces_feedback";
        private const string officeservice_QueueName = "officeservice_queue1";
        private const string workplaceservice_QueueName = "workplaceservice_queue1";
        #endregion
        #region RabbitMQ variables
        private readonly IModel channel;
        private readonly IBasicProperties officeservice_props;
        private readonly IBasicProperties workplaceservice_props;
        private readonly EventingBasicConsumer consumer;
        private readonly IRabbitMQPersistentConnection rabbitMQPersistentConnection;
        #endregion
        #region BlockingCollections
        private readonly BlockingCollection<WorkplaceResponse> workplaceservice_data = new BlockingCollection<WorkplaceResponse>();
        private readonly BlockingCollection<Office> officeservice_data = new BlockingCollection<Office>();
        private readonly BlockingCollection<IEnumerable<WorkplaceResponse>> workplaceservice_collection_data = new BlockingCollection<IEnumerable<WorkplaceResponse>>();
        #endregion

        #region private methods

        #region Office Message
        private Office Message(Guid officeguid)
        {
            var message = Encoding.UTF8.GetBytes(officeguid.ToString());
            #region Channel Settings
            channel.BasicPublish(
                exchange: "",
                routingKey: officeservice_QueueName,
                basicProperties: officeservice_props,
                body: message);
            channel.BasicConsume(
                consumer: consumer,
                queue: officeservice_replyQueueName,
                autoAck: true);
            #endregion
            var item = officeservice_data.Take();
            return item;
        }
        #endregion

        #region Workplace Message
        private WorkplaceResponse Message(int spaceid)
        {
            var message = Encoding.UTF8.GetBytes(spaceid.ToString());
            #region Channel Settings
            channel.BasicPublish(
                exchange: "",
                routingKey: workplaceservice_QueueName,
                basicProperties: workplaceservice_props,
                body: message);

            channel.BasicConsume(
                consumer: consumer,
                queue: workplaceservice_replyQueueName,
                autoAck: true);
            #endregion
            var item = workplaceservice_data.Take();
            return item;
        }
        #endregion
        #endregion

        public ServicesClient([FromServices] IRabbitMQPersistentConnection rabbitMQPersistentConnection)
        {
            this.rabbitMQPersistentConnection = rabbitMQPersistentConnection;
            channel = rabbitMQPersistentConnection.CreateModel();
            consumer = new EventingBasicConsumer(channel);

            channel.ExchangeDeclare("feedback", ExchangeType.Direct);

            #region OfficeService Queue Settings
            channel.QueueDeclare(queue: officeservice_replyQueueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: true);
            channel.QueueBind(officeservice_replyQueueName, "feedback", "data");
            channel.QueueBind(officeservice_replyQueueName, "feedback", "error");

            channel.ExchangeDeclare("exchange", ExchangeType.Direct);
            #endregion
            #region WorkplaceService Queue Settings
            channel.QueueDeclare(queue: workplaceservice_replyQueueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: true);
            channel.QueueBind(workplaceservice_replyQueueName, "feedback", "data");
            channel.QueueBind(workplaceservice_replyQueueName, "feedback", "error");
            channel.QueueBind(workplaceservice_replyQueueName, "feedback", "collection_data");
            #endregion
            #region Properties Setup
            workplaceservice_props = channel.CreateBasicProperties();
            officeservice_props = channel.CreateBasicProperties();
             
            officeservice_props.CorrelationId = Guid.NewGuid().ToString();
            workplaceservice_props.CorrelationId = Guid.NewGuid().ToString();

            officeservice_props.ReplyTo = Guid.NewGuid().ToString();
            workplaceservice_props.ReplyTo = Guid.NewGuid().ToString();
            #endregion

            consumer.Received += (model, ea) =>
            {
                // нужно ли заменить if на switch?
                if (ea.BasicProperties != null)
                { 
                    #region OfficeService sent a model
                    if (ea.RoutingKey == "data" && ea.BasicProperties.ReplyTo == officeservice_props.ReplyTo)
                    {
                        var body = ea.Body;
                        var response = Encoding.UTF8.GetString(body.ToArray());
                        var feedback = JsonConvert.DeserializeObject<Office>(response);
                        if (ea.BasicProperties.CorrelationId == officeservice_props.CorrelationId)
                        {
                            officeservice_data.Add(feedback);
                        }
                        Console.WriteLine("Office is received; BindingKey == data");
                    }

                #endregion
                    #region OfficeService sent an error
                        if (ea.RoutingKey == "error" && ea.BasicProperties.ReplyTo == officeservice_props.ReplyTo)
                        {
                        if (ea.BasicProperties.CorrelationId == officeservice_props.CorrelationId)
                        {
                            Office office = null;
                            officeservice_data.Add(office);
                            Console.WriteLine("Error is received; BindingKey == error");
                        }
                        // ErrorHandle Implementation
                    }
                    #endregion

                    #region WorkplaceService sent a model
                        if (ea.RoutingKey == "data" && ea.BasicProperties.ReplyTo == workplaceservice_props.ReplyTo)
                        {
                            if (ea.BasicProperties.CorrelationId == workplaceservice_props.CorrelationId)
                            {
                                var body = ea.Body;
                                var response = Encoding.UTF8.GetString(body.ToArray());
                                var feedback = JsonConvert.DeserializeObject<WorkplaceResponse>(response);
                                workplaceservice_data.Add(feedback);
                                Console.WriteLine("Workplace is received");
                            }
                        }
                    #endregion
                    #region WorkplaceService sent a model collection
                        if (ea.RoutingKey == "collection_data" && ea.BasicProperties.CorrelationId == workplaceservice_props.ReplyTo)
                        {
                        if (ea.BasicProperties.CorrelationId == workplaceservice_props.CorrelationId)
                        {
                            var body = ea.Body;
                            var response = Encoding.UTF8.GetString(body.ToArray());
                            var feedback = JsonConvert.DeserializeObject<IEnumerable<WorkplaceResponse>>(response);
                            workplaceservice_collection_data.Add(feedback);
                            Console.WriteLine("Workplaces are received");
                        }
                    }
                    #endregion
                    #region WorkplaceService sent an error
                        if (ea.RoutingKey == "error" && ea.BasicProperties.CorrelationId == workplaceservice_props.ReplyTo)
                        {
                        if (ea.BasicProperties.CorrelationId == workplaceservice_props.CorrelationId)
                        {
                            Console.WriteLine("404: Workplace is not found");
                        }
                        // ErrorHandle implementation
                    }
                    #endregion
                }
            };
        }

        #region Controller-invoked methods
        public Task<Office> GetOfficeAsync(Guid officeGuid)
        {
            Console.WriteLine("HTTP-request for office is sent");
            var item = Message(officeGuid);
            return Task.FromResult(item);
        }


            public Task<WorkplaceResponse> GetWorkplacesAsync(int spaceid)
        {
            Console.WriteLine("HTTP-request for workplaces is sent");
            var item = Message(spaceid);
            return Task.FromResult(item);
        }
        #endregion


        #region Close, Dispose
        public void Close()
        {
            rabbitMQPersistentConnection.Close();
        }

        public void Dispose()
        {
            rabbitMQPersistentConnection.Dispose();
        }
        #endregion
    }
}
