using RabbitMQ.Client;
using System;

namespace OfficeService.Messaging.RabbitMQ.Interface
{
    public interface IRabbitMQPersistentConnection : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();

        void Disconnect();
    }
}
