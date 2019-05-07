using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters.Dish
{
    public class DeleteDishParam
    {
        [Required]
        [JsonProperty("dishesIds")]
        public List<string> DishIds { get; set; }

        public string RestaurantId { get; set; }
    }
}
