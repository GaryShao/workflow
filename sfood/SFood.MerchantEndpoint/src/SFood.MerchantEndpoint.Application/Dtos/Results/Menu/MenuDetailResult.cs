using Newtonsoft.Json;
using System.Collections.Generic;

namespace SFood.MerchantEndpoint.Application.Dtos.Results.Menu
{
    public class MenuDetailResult
    {
        public MenuDetailResult()
        {
            Categories = new List<MenuDetail_CategoryResult>();
        }

        [JsonProperty("menuId")]
        public string Id { get; set; }

        [JsonProperty("menuName")]
        public string Name { get; set; }

        [JsonProperty("openedAt")]
        public short BeginTime { get; set; }

        [JsonProperty("closedAt")]
        public short EndTime { get; set; }

        public bool IsCurrent { get; set; }

        public int CountOfCategories { get; set; }

        public int CountOfDishes { get; set; }

        public List<MenuDetail_CategoryResult> Categories { get; set; }
    }

    public class MenuDetail_CategoryResult
    {
        public MenuDetail_CategoryResult()
        {
            Dishes = new List<MenuDetail_DishResult>();
        }

        [JsonProperty("categoryId")]
        public string Id { get; set; }

        [JsonProperty("categoryName")]
        public string Name { get; set; }

        [JsonProperty("categoryIndex")]
        public byte Index { get; set; }

        public List<MenuDetail_DishResult> Dishes { get; set; }
    }

    public class MenuDetail_DishResult
    {
        [JsonProperty("dishesId")]
        public string Id { get; set; }

        [JsonProperty("dishesName")]
        public string Name { get; set; }

        [JsonProperty("dishesIcon")]
        public string Icon { get; set; }

        [JsonProperty("dishesPrice")]
        public string UnitPrice { get; set; }

        [JsonProperty("dishesIndex")]
        public byte Index { get; set; }

        [JsonProperty("dishesIsOnShelf")]
        public bool IsOnShelf { get; set; }
    }
}
