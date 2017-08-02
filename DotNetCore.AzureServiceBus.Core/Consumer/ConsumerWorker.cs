using System;
using DotNetCore.AzureServiceBus.Core.Client;
using DotNetCoreRabbitMq.Infrastructure.Serializer;
using Microsoft.ServiceBus.Messaging;

namespace DotNetCore.AzureServiceBus.Core.Consumer
{
    public class ConsumerWorker<TService, TMessage>
        where TService : class, IMessageBusService<TMessage>
        where TMessage : class
    {
        private readonly string _connectionString;
        private readonly ConsumerProperties _consumerProperties;
        private readonly IMessageBusService<TMessage> _service;
        private readonly ISerializer _serializer;
        private SubscriptionClient _subscriptionClient;

        public ConsumerWorker(
            string connectionString,
            ConsumerProperties consumerProperties,
            IDependencyResolver dependencyResolver)
        {
            _connectionString = connectionString;
            _consumerProperties = consumerProperties;
            _service = dependencyResolver.Resolve<TService>();
            _serializer = dependencyResolver.Resolve<ISerializer>();
        }

        public void Start()
        {
            var factory = MessagingFactory.CreateFromConnectionString(_connectionString);
            _subscriptionClient = factory.CreateSubscriptionClient(_consumerProperties.QueueName, _consumerProperties.SubscriptionName);

            var options = new OnMessageOptions()
            {
                AutoComplete = false,
                MaxConcurrentCalls = _consumerProperties.ConsumersQuantity,
                AutoRenewTimeout = _consumerProperties.MessageLockTimeout
            };

            _subscriptionClient.OnMessage(Consume, options);
        }

        public void Stop()
        {
            _subscriptionClient.Close();
        }

        private void Consume(BrokeredMessage msg)
        {
            try
            {
                if (!CanReadThisMessage(msg))
                {
                    msg.DeadLetter("Consumer cannot read the message", "Sender/Serializer could not be recognized");
                    //TODO: log error ("Consumer cannot read the message because Sender/Serializer could not be recognized.");
                    return;
                }

                var message = _serializer.DeSerialize<TMessage>(msg.GetBody<byte[]>());

                _service.ProcessMessage(message);

                msg.Complete();
            }
            catch (Exception ex)
            {
                // TODO: create a flexibile way to handle exception
                //Console.WriteLine($"Queue unhandled exception. Type: {ex.GetType().Name}, Message: {ex.Message}");
                msg.Abandon();
            }
        }

        private bool CanReadThisMessage(BrokeredMessage msg)
        {
            return msg.ContentType == _serializer.ContentType;
        }
    }
}