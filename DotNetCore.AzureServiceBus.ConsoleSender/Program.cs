using System;
using System.Threading;
using DotNetCore.AzureServiceBus.Core;
using DotNetCore.AzureServiceBus.Core.Client;
using DotNetCoreRabbitMq.Infrastructure.Serializer;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace DotNetCore.AzureServiceBus.ConsoleSender
{
    class Program
    {
        static string ConnectionString = "Endpoint=sb://msgtest1.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=tESoO8O7NSpqFisstqFMwXCAydXSIhGDa8//tvaFcNI=";
        static string QueuePath = "demoqueue";
        static string TopicPath = "demotopic";
        static string SubscName = "demosubsc";


        static void Main(string[] args)
        {
            EnsureResourcesExists();

            //RunQueueSample();
            //RunTopicSample();
            RunMessageBusClientSample();
        }

        private static void RunMessageBusClientSample()
        {
            var connStringHolder = new ConnectionStringHolder(ConnectionString);
            var clientFactory = new MessageBusClientFactory(connStringHolder, new JsonSerializer());
            var client = clientFactory.CreateClient(TopicPath);

            for (int i = 0; i < 100; i++)
            {
                var message = new CustomMessage($"Message {i}", "admin", DateTime.Now);
                client.Send(message);
                Console.WriteLine("Sent: " + i);
            }

            client.Dispose();
        }

        private static void EnsureResourcesExists()
        {
            var manager = NamespaceManager.CreateFromConnectionString(ConnectionString);
            if (!manager.QueueExists(QueuePath))
            {
                manager.CreateQueue(QueuePath);
            }

            if (!manager.TopicExists(TopicPath))
            {
                manager.CreateTopic(TopicPath);
            }

            if (!manager.SubscriptionExists(TopicPath, SubscName))
            {
                manager.CreateSubscription(TopicPath, SubscName);
            }
        }

        private static void RunTopicSample()
        {
            // queues sample
            var factory = MessagingFactory.CreateFromConnectionString(ConnectionString);
            var topicClient = factory.CreateTopicClient(TopicPath);
            var subscriptionClient = factory.CreateSubscriptionClient(TopicPath, SubscName);

            SendTopicMessages(topicClient);

            //Console.WriteLine("Press any key to start receive messages");
            //Console.ReadLine();
            //ReceiveTopicMessages(subscriptionClient);

            Console.ReadLine();
            topicClient.Close();
            subscriptionClient.Close();
        }
        private static void RunQueueSample()
        {
            var queueClient = QueueClient.CreateFromConnectionString(ConnectionString, QueuePath);
            //SendQueueMessages(queueClient);

            Console.WriteLine("Press any key to start receive messages");
            Console.ReadLine();
            ReceiveQueueMessages(queueClient);

            Console.ReadLine();
            queueClient.Close();
        }

        private static void SendTopicMessages(TopicClient topicClient)
        {
            for (int i = 0; i < 100; i++)
            {
                var message = new BrokeredMessage("Topic Message: " + i);
                topicClient.Send(message);
                Console.WriteLine("Sent: " + i);
            }
        }
        private static void ReceiveTopicMessages(SubscriptionClient subscriptionClient)
        {
            subscriptionClient.OnMessage(message =>
            {
                var text = message.GetBody<string>();

                Console.WriteLine("Received: " + text);
            }
            , new OnMessageOptions());
        }

        private static void SendQueueMessages(QueueClient queueClient)
        {
            for (int i = 0; i < 100; i++)
            {
                var message = new BrokeredMessage("Message: " + i);
                queueClient.Send(message);
                Console.WriteLine("Sent: " + i);
            }
        }
        private static void ReceiveQueueMessages(QueueClient queueClient)
        {
            queueClient.OnMessage(message =>
            {
                var text = message.GetBody<string>().PadRight(12, ' ');

                Console.WriteLine($"Received: {text} - ThreadId: {Thread.CurrentThread.ManagedThreadId}");
            }
            , new OnMessageOptions() { MaxConcurrentCalls = 5 });
        }

    }

    public class CustomMessage
    {
        public string Content { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedOn { get; set; }

        public CustomMessage(string content, string userName, DateTime createdOn)
        {
            Content = content;
            UserName = userName;
            CreatedOn = createdOn;
        }

        public override string ToString()
        {
            return $"{nameof(Content)}: {Content}, {nameof(UserName)}: {UserName}, {nameof(CreatedOn)}: {CreatedOn}";
        }
    }
}