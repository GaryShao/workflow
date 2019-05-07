﻿using Newtonsoft.Json;

namespace SFood.ClientEndpoint.Common.Dtos.CommsHub
{
    public class TokenResult
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
