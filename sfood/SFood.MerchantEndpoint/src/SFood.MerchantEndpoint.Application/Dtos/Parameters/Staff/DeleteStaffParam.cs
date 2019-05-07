using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters.Staff
{
    public class DeleteStaffParam
    {
        [Required]
        [JsonProperty("staffId")]
        public string Id { get; set; }
    }
}
