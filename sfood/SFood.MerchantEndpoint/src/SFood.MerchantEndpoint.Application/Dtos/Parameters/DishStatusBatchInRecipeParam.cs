using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters
{
    public class DishStatusBatchInRecipeParam
    {
        [Required]
        public string MenuId { get; set; }

        [Required]
        [JsonProperty("dishesId")]
        public List<string> DishIds { get; set; }

        [Required]
        public bool SetAsOnShelf { get; set; }
    }
}
