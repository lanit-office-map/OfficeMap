using System;
using System.IO;
using System.Threading;
using Microsoft.Extensions.Logging;
using Common.RabbitMQ.Interface;
using RabbitMQ.Client;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client.Exceptions;
using RabbitMQ.Client.Events;

namespace Common.RabbitMQ
{
    public class RabbitMQPersistentConnection : IRabbitMQPersistentConnection
    {
        private readonly IConnectionFactory connectionFactory;
        private readonly ILogger<RabbitMQPersistentConnection> logger;
        private IConnection connection;
        private bool disposed;

        public RabbitMQPersistentConnection(
            [FromServices] IConnectionFactory connectionFactory,
            [FromServices] ILogger<RabbitMQPersistentConnection> logger)
        {
            this.connectionFactory = connectionFactory;
            this.logger = logger;
            logger.LogInformation("Connection is established");
            if (!IsConnected)
            {
                TryConnect();
            }
        }
        public void Disconnect()
        {
            if (disposed)
            {
                return;
            }
            Dispose();
        }

        public bool IsConnected
        {
            get
            {
                return connection != null && connection.IsOpen && !disposed;
            }
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }
            return connection.CreateModel();
        }
            public void Dispose()
        {
            if (disposed) return;

            disposed = true;

            try
            {
                connection.Dispose();
            }
            catch (IOException ex)
            {
                logger.LogInformation(ex.ToString());
            }
        }

        public bool TryConnect()
        {

            try
            {
                logger.LogInformation("RabbitMQ Client is trying to connect");
                connection = connectionFactory.CreateConnection();
            }
            catch (BrokerUnreachableException e)
            {
                Thread.Sleep(5000);
                logger.LogInformation("RabbitMQ Client is trying to reconnect");
                connection = connectionFactory.CreateConnection();
            }

            if (IsConnected)
            {
                connection.ConnectionShutdown += OnConnectionShutdown;
                connection.CallbackException += OnCallbackException;
                connection.ConnectionBlocked += OnConnectionBlocked;

                logger.LogInformation($"RabbitMQ persistent connection acquired a connection {connection.Endpoint.HostName} and is subscribed to officeguid_queue");

                return true;
            }
            else
            {
                logger.LogInformation("FATAL ERROR: RabbitMQ connections could not be created and opened");
                return false;
            }

        }

        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (disposed) return;
            logger.LogInformation("A RabbitMQ connection is shutdown. Trying to re-connect...");
            TryConnect();
        }

        void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (disposed) return;
            logger.LogInformation("A RabbitMQ connection throw exception. Trying to re-connect...");
            TryConnect();
        }

        void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
        {
            if (disposed) return;
            logger.LogInformation("A RabbitMQ connection is on shutdown. Trying to re-connect...");
            TryConnect();
        }
    }
}
