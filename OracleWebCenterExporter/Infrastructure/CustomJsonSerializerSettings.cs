using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace OracleWebCenterExporter.Infrastructure
{
    public class CustomJsonSerializerSettings
    {
        private CustomJsonSerializerSettings() { }

        public static CustomJsonSerializerSettings Instance { get; } = new CustomJsonSerializerSettings();

        public JsonSerializerSettings Settings { get; } = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            StringEscapeHandling = StringEscapeHandling.EscapeHtml,
            Formatting = Formatting.Indented
        };
    }
}
