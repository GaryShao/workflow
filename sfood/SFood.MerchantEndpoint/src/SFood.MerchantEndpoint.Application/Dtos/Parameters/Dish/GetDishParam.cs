using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters.Dish
{
    public class GetDishParam
    {
        [Required]
        public string DishesId { get; set; }
    }
}
