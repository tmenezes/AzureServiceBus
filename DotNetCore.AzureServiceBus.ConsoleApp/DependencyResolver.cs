using System;
using DotNetCore.AzureServiceBus.Core;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetCore.AzureServiceBus.ConsoleApp
{
    public class DependencyResolver : IDependencyResolver
    {
        private readonly IServiceProvider _container;

        public DependencyResolver(IServiceCollection services)
        {
            _container = services.BuildServiceProvider();
        }

        public T Resolve<T>() where T : class
        {
            return (T)_container.GetService(typeof(T));
        }
    }
}