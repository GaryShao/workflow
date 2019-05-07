using Newtonsoft.Json;
using System.Collections.Generic;

namespace SFood.MerchantEndpoint.Common.Dtos
{
    public class SendResult
    {
        [JsonProperty("has_error")]
        public bool HasError { get; set; }

        [JsonProperty("error_list")]
        public List<string> ErrorList { get; set; }
    }
}
