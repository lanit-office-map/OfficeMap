using RabbitMQ.Client;
using System;

namespace Common.RabbitMQ.Interface
{
    public interface IRabbitMQPersistentConnection : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();

        void Disconnect();
    }
}
