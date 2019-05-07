using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters.Dish
{
    public class EditCustomizationParam
    {
        public EditCustomizationParam()
        {
            CustomizationCategories = new List<CustomizationCategoryDto>();
        }

        [Required]
        public string DishId { get; set; }

        [JsonProperty("customizations")]
        public List<CustomizationCategoryDto> CustomizationCategories { get; set; }
    }
}
