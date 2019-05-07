using Newtonsoft.Json;

namespace SFood.MerchantEndpoint.Application.Dtos.Results
{
    public class RestaurantCategoryResult
    {
        [JsonProperty("categoryId")]
        public string Id { get; set; }

        [JsonProperty("categoryName")]
        public string Name { get; set; }
    }
}
