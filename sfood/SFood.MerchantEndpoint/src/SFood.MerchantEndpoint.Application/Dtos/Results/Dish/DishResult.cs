using Newtonsoft.Json;
using SFood.MerchantEndpoint.Application.Dtos.Parameters.Dish;
using System.Collections.Generic;

namespace SFood.MerchantEndpoint.Application.Dtos.Results.Dish
{
    public class DishResult
    {
        [JsonProperty("dishesId")]
        public string Id { get; set; }

        [JsonProperty("category")]
        public List<CategoryDto> Categories { get; set; }

        [JsonProperty("dishesName")]
        public string Name { get; set; }

        [JsonProperty("dishesPrice")]
        public string UnitPrice { get; set; }

        [JsonProperty("dishesIcon")]
        public string Icon { get; set; }

        [JsonProperty("specs")]
        public List<CustomizationCategoryDto> CustomizationCategories { get; set; }

        public class CategoryDto
        {
            [JsonProperty("categoryId")]
            public string Id { get; set; }

            [JsonProperty("categoryName")]
            public string Name { get; set; }

            public string MenuId { get; set; }

            public string MenuName { get; set; }
        }
    }
}
