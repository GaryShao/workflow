using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters
{
    public class CreateDishCategoryParam
    {
        public string MenuId { get; set; }

        [MaxLength(100)]
        [Required]
        [JsonProperty("categoryName")]
        public string Name { get; set; }

        public string RestaurantId { get; set; }
    }
}
