using System;
using System.Threading;
using DotNetCore.AzureServiceBus.Core;
using DotNetCore.AzureServiceBus.Core.Consumer;
using DotNetCore.AzureServiceBus.Core.Managment;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace DotNetCore.AzureServiceBus.ConsoleApp
{
    class Program
    {
        static string ConnectionString = "Endpoint=sb://msgtest1.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=tESoO8O7NSpqFisstqFMwXCAydXSIhGDa8//tvaFcNI=";
        static string TopicPath = "demotopic";
        static string SubscName = "demosubsc";
        static IDependencyResolver DependencyResolver = null;

        static void Main(string[] args)
        {
            SetupContainer();
            EnsureResourcesExists();

            var consumerFactory = new ConsumerFactory(ConnectionString, DependencyResolver);

            var consumerProperties1 = ConsumerProperties.ForMultipleConsumers(TopicPath, SubscName, 25);
            var consumer1 = consumerFactory.CreateConsumer<CustomService1, CustomMessage>(consumerProperties1);

            consumer1.Start();

            Console.WriteLine("Press any key to stop the app...");
            Console.Read();

            consumer1.Stop();
        }

        private static void SetupContainer()
        {
            var services = new ServiceCollection();

            services.AddMessageBus(ConnectionString)
                    .AddSingleton(new CustomService1());

            DependencyResolver = new DependencyResolver(services);
        }
        private static void EnsureResourcesExists()
        {
            var messageBuseManager = DependencyResolver.Resolve<IMessageBusManager>();

            messageBuseManager.CreateTopic(TopicPath);
            messageBuseManager.CreateSubscriber(TopicPath, SubscName);
        }
    }
}