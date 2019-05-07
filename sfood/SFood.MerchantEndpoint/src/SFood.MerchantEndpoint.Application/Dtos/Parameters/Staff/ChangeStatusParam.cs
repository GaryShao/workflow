using Newtonsoft.Json;
using SFood.DataAccess.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters.Staff
{
    public class ChangeStatusParam
    {
        [Required]
        [JsonProperty("staffId")]
        public string Id { get; set; }

        [Required]
        [JsonProperty("staffState")]
        public StaffStatus Status { get; set; }
    }
}
