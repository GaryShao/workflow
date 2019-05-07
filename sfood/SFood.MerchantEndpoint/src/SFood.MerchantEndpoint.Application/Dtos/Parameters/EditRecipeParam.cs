using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters
{
    public class EditRecipeParam
    {
        [Required]
        [JsonProperty("menuId")]
        public string Id { get; set; }

        /// <summary>
        /// 菜单的名字
        /// </summary>
        [Required]
        [MaxLength(20)]
        [JsonProperty("menuName")]
        public string Name { get; set; }

        /// <summary>
        /// 菜单的生效时间
        /// </summary>
        [Range(0, 1440)]
        [JsonProperty("openedAt")]
        public short BeginTime { get; set; }

        /// <summary>
        /// 菜单的失效时间
        /// </summary>
        [Range(0, 1440)]
        [JsonProperty("closedAt")]
        public short EndTime { get; set; }

        /// <summary>
        /// 餐厅id
        /// </summary>
        public string RestaurantId { get; set; }
    }
}
