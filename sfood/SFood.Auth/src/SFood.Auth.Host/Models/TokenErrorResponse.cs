using Newtonsoft.Json;

namespace SFood.Auth.Host.Models
{
    public class TokenErrorResponse
    {
        [JsonProperty("error")]
        public string Error { get; set; }

        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }
    }
}
