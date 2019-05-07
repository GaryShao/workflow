using Newtonsoft.Json;

namespace SFood.MerchantEndpoint.Application.Dtos.Results.HawkerCenter
{
    public class HawkerCenterResult
    {
        [JsonProperty("centerId")]
        public string Id { get; set; }

        [JsonProperty("centerName")]
        public string Name { get; set; }
    }
}
