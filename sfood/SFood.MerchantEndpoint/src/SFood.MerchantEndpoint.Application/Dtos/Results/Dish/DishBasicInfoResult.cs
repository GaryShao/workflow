using Newtonsoft.Json;

namespace SFood.MerchantEndpoint.Application.Dtos.Results.Dish
{
    public class DishBasicInfoResult
    {
        [JsonProperty("dishesId")]
        public string Id { get; set; }

        [JsonProperty("dishesName")]
        public string Name { get; set; }

        [JsonProperty("dishesPrice")]
        public decimal UnitPrice { get; set; }

        [JsonProperty("dishesIcon")]
        public string Icon { get; set; }

        [JsonProperty("dishesIndex")]
        public byte Index { get; set; }

        [JsonProperty("dishesIsOnShelf")]
        public bool IsOnShelf { get; set; }
    }
}
