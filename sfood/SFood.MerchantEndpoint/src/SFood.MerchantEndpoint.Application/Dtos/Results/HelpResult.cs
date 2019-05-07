using Newtonsoft.Json;

namespace SFood.MerchantEndpoint.Application.Dtos.Results
{
    public class HelpResult
    {
        [JsonProperty("helpId")]
        public string Id { get; set; }

        [JsonProperty("helpName")]
        public string Name { get; set; }

        [JsonProperty("helpUrl")]
        public string Url { get; set; }
    }
}
