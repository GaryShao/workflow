using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters
{
    public class RestaurantConfigurationParam
    {
        public string RestaurantId { get; set; }

        [Required]
        [JsonProperty("startTime")]
        public short OpenedAt { get; set; }

        [Required]
        [JsonProperty("endTime")]
        public short ClosedAt { get; set; }

        public bool IsAutoReceiving { get; set; }

        [Required]
        [JsonProperty("effectTime")]
        public byte OrderResponseTime { get; set; }

        public bool IsDeliverySupport { get; set; }
    }
}
