namespace DotNetCore.AzureServiceBus.Core.Consumer
{
    public interface IMessageBusService<T> where T : class
    {
        void ProcessMessage(T message);
    }
}
