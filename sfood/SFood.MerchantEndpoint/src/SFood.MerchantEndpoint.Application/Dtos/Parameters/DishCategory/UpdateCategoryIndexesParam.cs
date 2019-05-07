using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters.DishCategory
{
    public class UpdateCategoryIndexesParam
    {
        [Required]
        public string MenuId { get; set; }

        [Required]
        [JsonProperty("sequentialIds")]
        public List<string> CategoryIds { get; set; }
    }
}
