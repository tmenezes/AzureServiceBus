namespace DotNetCore.AzureServiceBus.Core
{
    public class ConnectionStringHolder
    {
        private readonly string _connectionString;

        public ConnectionStringHolder(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string Get() => _connectionString;

        public override string ToString() => Get();
    }
}