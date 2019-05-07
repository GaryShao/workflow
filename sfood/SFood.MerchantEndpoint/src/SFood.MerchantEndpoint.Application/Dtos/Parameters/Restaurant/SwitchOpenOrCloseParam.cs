using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters.Restaurant
{
    public class SwitchOpenOrCloseParam
    {
        public string RestaurantId { get; set; }
        
        [Required]
        [JsonProperty("setAsOpened")]
        public bool ToOpened { get; set; }
    }
}
