using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters.Dish
{
    public class UpdateDishIndexParam
    {
        [Required]
        [JsonProperty("dishCategoryId")]
        public string CategoryId { get; set; }

        [Required]
        public List<string> DishIds { get; set; }
    }
}
