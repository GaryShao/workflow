using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SFood.ClientEndpoint.Application.Dtos.Parameter
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
