using Newtonsoft.Json;
using SFood.DataAccess.Common.Enums;

namespace SFood.Auth.Host.Models
{
    public class FetchTokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("registration_status")]
        public RestaurantRegistrationStatus? RegistrationStatus { get; set; }

        [JsonProperty("ever_logined")]
        public bool HasUserEverLogined { get; set; }

        [JsonProperty("is_auto_receiving")]
        public bool? IsAutoReceiving { get; set; }
    }
}
