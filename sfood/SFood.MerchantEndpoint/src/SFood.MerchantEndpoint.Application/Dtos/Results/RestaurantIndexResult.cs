using Newtonsoft.Json;
using SFood.MerchantEndpoint.Common.Enums;

namespace SFood.MerchantEndpoint.Application.Dtos.Results
{
    public class RestaurantIndexResult
    {
        public string Logo { get; set; }

        public string Name { get; set; }

        [JsonProperty("auditState")]
        public RestaurantAuditStatus AuditStatus { get; set; }

        public string Phone { get; set; }
    }
}
