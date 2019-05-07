using Newtonsoft.Json;

namespace SFood.MerchantEndpoint.Application.Dtos.Results.Menu
{
    public class MenuBasicResult
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
    }
}
