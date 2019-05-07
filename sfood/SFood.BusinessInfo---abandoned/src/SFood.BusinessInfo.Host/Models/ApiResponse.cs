using Newtonsoft.Json;
using SFood.BusinessInfo.Common.Enums;

namespace SFood.BusinessInfo.Host.Models
{
    public class ApiResponse
    {
        [JsonProperty(PropertyName = "business_status_code")]
        public BusinessStatusCode StatusCode { get; set; }

        public object Data { get; set; }
    }
}
