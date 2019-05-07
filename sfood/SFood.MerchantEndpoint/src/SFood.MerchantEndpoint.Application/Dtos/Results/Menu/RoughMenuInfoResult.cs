using Newtonsoft.Json;
using System.Collections.Generic;

namespace SFood.MerchantEndpoint.Application.Dtos.Results.Menu
{
    public class RoughMenuInfoResult
    {
        [JsonProperty("menuId")]
        public string Id { get; set; }

        [JsonProperty("menuName")]
        public string Name { get; set; }

        [JsonProperty("catalogs")]
        public List<CategoryInfo_RoughMenu> Categories { get; set; }
    }

    public class CategoryInfo_RoughMenu
    {
        [JsonProperty("categoryId")]
        public string Id { get; set; }

        [JsonProperty("categoryName")]
        public string Name { get; set; }
    }
}
