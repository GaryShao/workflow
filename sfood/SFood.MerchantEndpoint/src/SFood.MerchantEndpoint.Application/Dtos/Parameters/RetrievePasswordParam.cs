using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters
{
    public class RetrievePasswordParam
    {
        [Required]
        [JsonProperty("countryId")]
        public string CountryCodeId { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        [RegularExpression(@"(?=.*[0-9])(?=.*[a-zA-Z]).{6,12}",
            ErrorMessage = "Password must contain number, capital or lower-case letter, symbol; more than 6 and less than 12 letters")]
        public string Password { get; set; }
    }
}
