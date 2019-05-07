using Newtonsoft.Json;
using System.Collections.Generic;

namespace SFood.ClientEndpoint.Application.Dtos
{
    public class SMSSendResponse
    {
        [JsonProperty("has_error")]
        public bool HasError { get; set; }

        [JsonProperty("error_list")]
        public List<string> ErrorList { get; set; }
    }
}
