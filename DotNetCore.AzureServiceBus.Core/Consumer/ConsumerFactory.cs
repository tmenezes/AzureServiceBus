using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetCore.AzureServiceBus.Core.Consumer
{
    public class ConsumerFactory
    {
        private readonly string _connectionString;
        private readonly IDependencyResolver _container;

        public ConsumerFactory(ConnectionStringHolder connectionStringHolder, IDependencyResolver container)
            : this(connectionStringHolder.Get(), container)
        {
        }
        public ConsumerFactory(string connectionString, IDependencyResolver container)
        {
            _connectionString = connectionString;
            _container = container;
        }

        public ConsumerWorker<TService, TMessage> CreateConsumer<TService, TMessage>(ConsumerProperties consumerProperties)
            where TService : class, IMessageBusService<TMessage>
            where TMessage : class
        {
            return new ConsumerWorker<TService, TMessage>(_connectionString, consumerProperties, _container);
        }
    }
}
