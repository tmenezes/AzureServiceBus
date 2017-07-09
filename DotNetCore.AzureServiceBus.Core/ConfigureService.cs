using DotNetCore.AzureServiceBus.Core.Client;
using DotNetCore.AzureServiceBus.Core.Managment;
using DotNetCoreRabbitMq.Infrastructure.Serializer;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetCore.AzureServiceBus.Core
{
    public static class ConfigureService
    {
        public static IServiceCollection AddMessageBus(this IServiceCollection services, string connString, ISerializer serializer = null)
        {
            services.AddSingleton(new ConnectionStringHolder(connString));
            services.AddSingleton<IMessageBusClientFactory, MessageBusClientFactory>();
            services.AddSingleton<IMessageBusManager, MessageBusManager>();
            services.AddSingleton(serializer ?? new JsonSerializer());

            return services;
        }
    }
}
