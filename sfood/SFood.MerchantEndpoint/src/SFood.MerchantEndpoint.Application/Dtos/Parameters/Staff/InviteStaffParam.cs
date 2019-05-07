using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters.Staff
{
    public class InviteStaffParam
    {        
        public string RestaurantId { get; set; }

        [Required]
        [JsonProperty("staffName")]
        public string Name { get; set; }

        [Required]
        [JsonProperty("staffPhone")]
        public string Phone { get; set; }

        [Required]
        [JsonProperty("countryId")]
        public string CountryCodeId { get; set; }
    }
}
