using Newtonsoft.Json;
using SFood.DataAccess.Common.Enums;

namespace SFood.MerchantEndpoint.Application.Dtos.Results.Staff
{
    public class StaffResult
    {
        [JsonProperty("staffId")]
        public string Id { get; set; }

        [JsonProperty("staffName")]
        public string Name { get; set; }

        [JsonProperty("staffPhone")]
        public string Phone { get; set; }

        [JsonProperty("staffState")]
        public StaffStatus Status { get; set; }

        public string CountryCode { get; set; }
    }
}
