using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetCoreRabbitMq.Infrastructure.Serializer;
using Microsoft.ServiceBus.Messaging;

namespace DotNetCore.AzureServiceBus.Core.Client
{
    public interface IMessageBusClientFactory
    {
        IMessageBusClient CreateClient(string topicPath);
    }

    public class MessageBusClientFactory : IMessageBusClientFactory
    {
        private readonly string _connectionString;
        private readonly ISerializer _serializer;

        public MessageBusClientFactory(ConnectionStringHolder connectionStringHolder, ISerializer serializer)
        {
            _connectionString = connectionStringHolder.Get();
            _serializer = serializer;
        }

        public IMessageBusClient CreateClient(string topicPath)
        {
            var factory = MessagingFactory.CreateFromConnectionString(_connectionString);
            var topicClient = factory.CreateTopicClient(topicPath);

            return new MessageBusClient(topicClient, _serializer);
        }
    }
}
