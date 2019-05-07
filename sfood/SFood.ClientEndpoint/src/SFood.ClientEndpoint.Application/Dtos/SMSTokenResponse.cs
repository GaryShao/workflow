﻿using Newtonsoft.Json;

namespace SFood.ClientEndpoint.Application.Dtos
{
    public class SMSTokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
