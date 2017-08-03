using System;
using System.Threading;
using DotNetCore.AzureServiceBus.Core.Consumer;

namespace DotNetCore.AzureServiceBus.ConsoleApp
{
    public class CustomService1 : IMessageBusService<CustomMessage>
    {
        public void ProcessMessage(CustomMessage message)
        {
            Log($"Service {nameof(CustomService1)} running the message: {message}");
            //Thread.Sleep(1001 * 60 * 5); // 5 min
            //throw new Exception("error by purposes");
            Log("Done!");
        }

        private void Log(string text) => Console.WriteLine($"{DateTime.Now} - ThreadId: {Thread.CurrentThread.ManagedThreadId} - {text}");
    }
}