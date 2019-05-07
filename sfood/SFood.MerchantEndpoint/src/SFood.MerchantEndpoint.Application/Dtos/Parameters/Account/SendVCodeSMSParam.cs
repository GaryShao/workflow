using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters.Account
{
    public class SendVCodeSMSParam
    {
        [Required]
        public string Phone { get; set; }

        [Required]
        [JsonProperty("countryId")]
        public string CountryCodeId { get; set; }

        public string Content { get; set; }
    }
}
