using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters.Menu
{
    public class DeleteDishCategoryParam
    {
        [Required]
        public string CategoryId { get; set; }
    }
}
