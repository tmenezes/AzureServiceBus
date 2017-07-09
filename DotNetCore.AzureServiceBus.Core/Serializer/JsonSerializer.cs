using System.Text;
using Newtonsoft.Json;

namespace DotNetCoreRabbitMq.Infrastructure.Serializer
{
    public class JsonSerializer : ISerializer
    {
        public byte[] Serialize(object obj)
        {
            if (obj == null)
                return null;

            var json = JsonConvert.SerializeObject(obj);
            return Encoding.ASCII.GetBytes(json);
        }

        public T DeSerialize<T>(byte[] arrBytes)
        {
            var json = Encoding.ASCII.GetString(arrBytes);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public string ContentType => "application/json";
    }
}