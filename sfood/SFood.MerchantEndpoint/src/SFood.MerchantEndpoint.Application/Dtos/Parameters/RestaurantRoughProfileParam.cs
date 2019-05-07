using Newtonsoft.Json;
using System.Collections.Generic;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters
{
    public class RestaurantRoughProfileParam
    {
        public RestaurantRoughProfileParam()
        {
            CategoryIds = new List<string>();
        }

        public string RestaurantId { get; set; }

        [JsonProperty("imageUrl")]
        public string Logo { get; set; }

        public string Name { get; set; }

        [JsonProperty("countryId")]
        public string CountryCodeId { get; set; }

        [JsonProperty("location")]
        public string RestaurantNo { get; set; }

        public string Phone { get; set; }

        [JsonProperty("catalog")]
        public List<string> CategoryIds { get; set; }
    }
}
