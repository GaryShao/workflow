using System.ComponentModel.DataAnnotations;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters.Menu
{
    public class DeleteMenuParam
    {
        [Required]
        public string MenuId { get; set; }
    }
}
