using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AzureEventHub
{
    public static class GlobalSetting
    {
        public static readonly JsonSerializerSettings JsonSetting = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            DateTimeZoneHandling = DateTimeZoneHandling.Unspecified,
        };
    }
}
