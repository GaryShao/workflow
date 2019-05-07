using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters.Dish
{
    public class CreateDishParam
    {
        public CreateDishParam()
        {
            CustomizationCategoryIds = new List<string>();
        }

        public string RestaurantId { get; set; }
        
        /// <summary>
        /// 菜品要添加到哪个菜单的哪个目录
        /// </summary>        
        [Required]
        public List<string> Categories { get; set; }

        [Required]
        [JsonProperty("dishesName")]
        public string Name { get; set; }

        [Required]
        [JsonProperty("dishesPrice")]
        public decimal UnitPrice { get; set; }

        [Required]
        [JsonProperty("dishesIcon")]
        public string Icon { get; set; }

        [JsonProperty("specIds")]
        public List<string> CustomizationCategoryIds { get; set; }
    }

    public class CustomizationCategoryDto
    {
        public CustomizationCategoryDto()
        {
            Options = new List<CustomizationDto>();
        }

        [JsonProperty("specId")]
        public string Id { get; set; }        

        [JsonProperty("specName")]
        public string Name { get; set; }

        [JsonProperty("isPreset")]
        public bool IsSystem { get; set; }

        public bool IsSelected { get; set; }

        [JsonProperty("isMultiSelect")]
        public bool IsMultiple { get; set; }

        [JsonProperty("maxSelectedNum")]
        public byte MaxSelected { get; set; }

        [JsonProperty("selections")]
        public List<CustomizationDto> Options { get; set; }
    }

    public class CustomizationDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal UnitPrice { get; set; }

        public bool IsDefault { get; set; }
    }
}
