using System;
using System.ServiceModel.PeerResolvers;

namespace DotNetCore.AzureServiceBus.Core.Consumer
{
    public class ConsumerProperties
    {
        public string QueueName { get; set; }
        public string SubscriptionName { get; }
        public int ConsumersQuantity { get; set; }
        public TimeSpan MessageLockTimeout { get; set; }

        //TODO: Implement support a subscriber with filtering of messages
        //public string Filtering { get; set; }
        //public AutoScale AutoScale { get; set; }
        //public ExceptionHandlingStrategy ExceptionHandlingStrategy { get; set; }


        public ConsumerProperties(string queueName, string subscriptionName, int consumersQuantity)
        {
            QueueName = queueName;
            SubscriptionName = subscriptionName;
            ConsumersQuantity = consumersQuantity;
            MessageLockTimeout = TimeSpan.FromMinutes(5);
        }

        public static ConsumerProperties ForSingleConsumer(string queueName, string subscriptionName)
        {
            return new ConsumerProperties(queueName, subscriptionName, 1);
        }

        public static ConsumerProperties ForMultipleConsumers(string queueName, string subscriptionName, int consumersQuantity)
        {
            return new ConsumerProperties(queueName, subscriptionName, consumersQuantity);
        }
    }
}
