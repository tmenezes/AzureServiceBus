namespace DotNetCoreRabbitMq.Infrastructure.Serializer
{
    public interface ISerializer
    {
        byte[] Serialize(object obj);
        T DeSerialize<T>(byte[] arrBytes);
        string ContentType { get; }
    }
}
