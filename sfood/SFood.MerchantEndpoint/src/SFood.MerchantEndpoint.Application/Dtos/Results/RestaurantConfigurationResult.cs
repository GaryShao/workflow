using Newtonsoft.Json;

namespace SFood.MerchantEndpoint.Application.Dtos.Results
{
    public class RestaurantConfigurationResult
    {
        [JsonProperty("startTime")]
        public short? OpenedAt { get; set; }

        [JsonProperty("endTime")]
        public short? ClosedAt { get; set; }

        public bool IsAutoReceiving { get; set; }

        [JsonProperty("effectTime")]
        public byte? OrderResponseTime { get; set; }

        public bool IsDeliverySupport { get; set; }
    }
}
