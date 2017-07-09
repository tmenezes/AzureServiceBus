namespace DotNetCore.AzureServiceBus.Core
{
    public interface IDependencyResolver
    {
        T Resolve<T>() where T : class;
    }
}
