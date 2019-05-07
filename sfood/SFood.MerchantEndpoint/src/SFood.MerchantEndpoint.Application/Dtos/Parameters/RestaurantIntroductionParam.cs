using Newtonsoft.Json;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters
{
    public class RestaurantIntroductionParam
    {
        public string RestaurantId { get; set; }

        [JsonProperty("brand")]
        public string Introduction { get; set; }
    }
}
