using Newtonsoft.Json;

namespace SFood.MerchantEndpoint.Application.Dtos.Results.Customization
{
    public class OptionResult
    {
        [JsonProperty("selectionId")]
        public string Id { get; set; }

        public string Name { get; set; }

        public byte Index { get; set; }

        public string UnitPrice { get; set; }

        public bool IsDefault { get; set; }
    }
}
