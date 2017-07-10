using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCoreRabbitMq.Infrastructure.Serializer;
using Microsoft.ServiceBus.Messaging;

namespace DotNetCore.AzureServiceBus.Core.Client
{
    public interface IMessageBusClient : IDisposable
    {
        void Send(object message, IDictionary<string, object> properties = null);
        Task SendAsync(object message, IDictionary<string, object> properties = null);
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

        public void Send(object message, IDictionary<string, object> properties = null)
        {
            var msg = GetMessage(message, properties);
            _client.Send(msg);
        }

        public async Task SendAsync(object message, IDictionary<string, object> properties = null)
        {
            var msg = GetMessage(message, properties);
            await _client.SendAsync(msg);
        }

        public void Dispose()
        {
            _client.Close();
        }

        private BrokeredMessage GetMessage(object message, IDictionary<string, object> properties = null)
        {
            var msg = new BrokeredMessage(_serializer.Serialize(message))
            {
                ContentType = _serializer.ContentType,
                Label = message.GetType().FullName
            };
            msg.Properties.Add("_Sender", _senderFullName);
            msg.Properties.Add("_Serializer", _serializerFullName);

            if (properties?.Any() ?? false)
            {
                foreach (var item in properties)
                {
                    msg.Properties.Add(item);
                }
            }

            return msg;
        }
    }
}
