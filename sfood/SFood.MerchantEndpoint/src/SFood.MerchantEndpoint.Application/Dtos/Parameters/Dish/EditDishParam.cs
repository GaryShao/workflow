using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters.Dish
{
    public class EditDishParam
    {
        [JsonProperty("categories")]
        public List<string> CategoryIds { get; set; }

        [JsonProperty("specIds")]
        public List<string> CustomizationCategoryIds { get; set; }

        [JsonProperty("dishesId")]
        [Required]
        public string Id { get; set; }

        [JsonProperty("dishesName")]
        [Required]
        public string Name { get; set; }

        [JsonProperty("dishesPrice")]
        [Required]
        public decimal UnitPrice { get; set; }

        [JsonProperty("dishesIcon")]
        [Required]
        public string Icon { get; set; }
    }
}
