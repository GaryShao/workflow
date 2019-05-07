using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters
{
    public class FetchSMSTokenParam
    {        
        [Required]
        [JsonProperty("type")]
        public string Type { get; set; }

        [Required]
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [Required]
        [JsonProperty("nonce")]
        public string Nonce { get; set; }

        [JsonProperty("Sign")]
        public string Sign { get; set; }
    }
}
