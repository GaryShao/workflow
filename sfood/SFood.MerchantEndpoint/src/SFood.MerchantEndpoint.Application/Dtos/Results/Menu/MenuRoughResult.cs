using Newtonsoft.Json;

namespace SFood.MerchantEndpoint.Application.Dtos.Results.Menu
{
    public class MenuRoughResult
    {
        /// <summary>
        /// 菜单的id
        /// </summary>
        [JsonProperty("menuId")]
        public string Id { get; set; }

        /// <summary>
        /// 菜单的名字
        /// </summary>
        [JsonProperty("menuName")]
        public string Name { get; set; }

        /// <summary>
        /// 菜单的生效时间
        /// </summary>
        [JsonProperty("openedAt")]
        public short BeginTime { get; set; }

        /// <summary>
        /// 菜单的失效时间
        /// </summary>
        [JsonProperty("closedAt")]
        public short EndTime { get; set; }
        
        /// <summary>
        /// 分类数目
        /// </summary>
        public int CountOfCategories { get; set; }

        /// <summary>
        /// 菜品数目
        /// </summary>
        public int CountOfDishes { get; set; }

        /// <summary>
        /// 是否是当前菜单
        /// </summary>
        public bool IsCurrent { get; set; }
    }
}
