using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters
{
    public class EditDishCategoryParam
    {
        [Required]
        [JsonProperty("categoryId")]
        public string Id { get; set; }

        [MaxLength(100)]
        [Required]
        [JsonProperty("categoryName")]
        public string Name { get; set; }
    }
}
