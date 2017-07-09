using System;
using DotNetCore.AzureServiceBus.Core.Consumer;

namespace DotNetCore.AzureServiceBus.ConsoleApp
{
    public class CustomService1 : IMessageBusService<CustomMessage>
    {
        public void ProcessMessage(CustomMessage message)
        {
            Console.WriteLine($"Service {nameof(CustomService1)} ran the message: {message}");
        }
    }
}