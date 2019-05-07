using System.ComponentModel.DataAnnotations;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters.Restaurant
{
    public class CreateStaffParam
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Phone { get; set; }
    }
}
