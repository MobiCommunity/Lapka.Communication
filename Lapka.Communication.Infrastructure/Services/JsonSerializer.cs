using Newtonsoft.Json;
using Lapka.Communication.Application.Services;

namespace Lapka.Communication.Infrastructure.Services
{
    public class CurrentJsonSerializer : IJsonSerializer
    {
        public string Serialize(object instance)
            => JsonConvert.SerializeObject(instance);

        public TResult Deserialize<TResult>(string value)
            => JsonConvert.DeserializeObject<TResult>(value);
    }
}