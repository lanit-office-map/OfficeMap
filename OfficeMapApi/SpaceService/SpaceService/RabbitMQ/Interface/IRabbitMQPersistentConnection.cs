using RabbitMQ.Client;
using System;

namespace SpaceService.RabbitMQ.Interface
{
    public interface IRabbitMQPersistentConnection : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();

        void Disconnect();

        void Close();

    }
}
