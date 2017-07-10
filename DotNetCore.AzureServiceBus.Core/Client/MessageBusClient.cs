using System;
using System.Threading.Tasks;
using DotNetCoreRabbitMq.Infrastructure.Serializer;
using Microsoft.ServiceBus.Messaging;

namespace DotNetCore.AzureServiceBus.Core.Client
{
    public interface IMessageBusClient : IDisposable
    {
        void Send(object message);
        Task SendAsync(object message);
    }

    public class MessageBusClient : IMessageBusClient
    {
        private readonly TopicClient _client;
        private readonly ISerializer _serializer;

        private readonly string _senderFullName = typeof(MessageBusClient).FullName;
        private readonly string _serializerFullName;

        public MessageBusClient(TopicClient client, ISerializer serializer)
        {
            _client = client;
            _serializer = serializer;

            _serializerFullName = serializer.GetType().FullName;
        }

        public void Send(object message)
        {
            var msg = GetMessage(message);
            _client.Send(msg);
        }

        public async Task SendAsync(object message)
        {
            var msg = GetMessage(message);
            await _client.SendAsync(msg);
        }

        public void Dispose()
        {
            _client.Close();
        }

        private BrokeredMessage GetMessage(object message)
        {
            var msg = new BrokeredMessage(_serializer.Serialize(message))
            {
                ContentType = _serializer.ContentType,
                Label = message.GetType().FullName
            };
            msg.Properties.Add("Sender", _senderFullName);
            msg.Properties.Add("Serializer", _serializerFullName);

            return msg;
        }
    }
}
