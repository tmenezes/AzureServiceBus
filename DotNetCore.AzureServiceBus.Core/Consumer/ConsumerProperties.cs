using System.ServiceModel.PeerResolvers;

namespace DotNetCore.AzureServiceBus.Core.Consumer
{
    public class ConsumerProperties
    {
        public string QueueName { get; private set; }
        public string SubscriptionName { get; }
        public int ConsumersQuantity { get; private set; }

        //TODO: Implement support a subscriber with filtering of messages
        //public string Filtering { get; set; }
        //public AutoScale AutoScale { get; set; }
        //public ExceptionHandlingStrategy ExceptionHandlingStrategy { get; set; }


        public ConsumerProperties(string queueName, string subscriptionName, int consumersQuantity)
        {
            QueueName = queueName;
            SubscriptionName = subscriptionName;
            ConsumersQuantity = consumersQuantity;
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
