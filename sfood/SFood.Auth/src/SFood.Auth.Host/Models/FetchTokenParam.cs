using Newtonsoft.Json;

namespace SFood.Auth.Host.Models
{
    public class FetchTokenParam
    {
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }

        [JsonProperty("grant_type")]
        public string GrantType { get; set; } = "password";

        [JsonProperty("username")]
        public string Phone { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("countryId")]
        public string CountryCodeId { get; set; }

        [JsonProperty("code")]
        public string VCode { get; set; }
    }
}
