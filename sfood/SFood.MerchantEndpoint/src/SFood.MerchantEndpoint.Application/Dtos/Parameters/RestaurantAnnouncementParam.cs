using Newtonsoft.Json;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters
{
    public class RestaurantAnnouncementParam
    {
        public string RestaurantId { get; set; }

        [JsonProperty("notice")]
        public string Announcement { get; set; }
    }
}
