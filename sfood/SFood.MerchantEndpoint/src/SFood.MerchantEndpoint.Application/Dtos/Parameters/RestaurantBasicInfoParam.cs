using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters
{
    public class RestaurantBasicInfoParam
    {
        public string RestaurantId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [JsonProperty("catalogIds")]
        public List<string> CategoryIds { get; set; }

        [Required]
        public string CenterId { get; set; }

        [Required]
        [MaxLength(20)]
        public string RestaurantNo { get; set; }
    }
}
