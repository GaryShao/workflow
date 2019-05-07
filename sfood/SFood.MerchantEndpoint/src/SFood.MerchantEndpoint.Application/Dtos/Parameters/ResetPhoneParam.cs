using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters
{
    public class ResetPhoneParam
    {
        [Required]
        public string NewPhone { get; set; }

        [Required]
        public string NewCode { get; set; }

        [JsonProperty("countryId")]
        public string CountryCodeId { get; set; }
    }
}
