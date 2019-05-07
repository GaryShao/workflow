using Newtonsoft.Json;

namespace SFood.MerchantEndpoint.Application.Dtos.Results.DishCategory
{
    public class DishCategoryResult
    {
        [JsonProperty("categoryId")]
        public string Id { get; set; }

        [JsonProperty("categoryName")]
        public string Name { get; set; }

        [JsonProperty("categoryIndex")]
        public byte Index { get; set; }

        public int CountOfDishes { get; set; }
    }
}
