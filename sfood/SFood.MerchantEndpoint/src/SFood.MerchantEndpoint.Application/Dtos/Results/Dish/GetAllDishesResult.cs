using Newtonsoft.Json;
using System.Collections.Generic;

namespace SFood.MerchantEndpoint.Application.Dtos.Results.Dish
{
    public class GetAllDishesResult
    {
        public GetAllDishesResult()
        {
            Menus = new List<MenuInfo>();
        }

        [JsonProperty("dishesId")]
        public string Id { get; set; }

        [JsonProperty("dishesName")]
        public string Name { get; set; }

        [JsonProperty("dishesPrice")]        
        public string UnitPrice { get; set; }

        [JsonProperty("dishesIcon")]
        public string Icon { get; set; }

        public List<MenuInfo> Menus { get; set; }
    }

    public class MenuInfo
    {
        [JsonProperty("menuName")]
        public string Name { get; set; }
    }
}
