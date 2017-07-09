using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus;

namespace DotNetCore.AzureServiceBus.Core.Managment
{
    public interface IMessageBusManager
    {
        void CreateTopic(string topicPath);
        void CreateSubscriber(string topicPath, string subscriberName);
    }

    public class MessageBusManager : IMessageBusManager
    {
        private readonly NamespaceManager _manager;

        public MessageBusManager(ConnectionStringHolder connectionStringHolder)
        {
            _manager = NamespaceManager.CreateFromConnectionString(connectionStringHolder.Get());
        }

        public void CreateTopic(string topicPath)
        {
            if (!_manager.TopicExists(topicPath))
            {
                _manager.CreateTopic(topicPath);
            }
        }

        public void CreateSubscriber(string topicPath, string subscriberName)
        {
            if (!_manager.SubscriptionExists(topicPath, subscriberName))
            {
                _manager.CreateSubscription(topicPath, subscriberName);
            }
        }
    }
}
