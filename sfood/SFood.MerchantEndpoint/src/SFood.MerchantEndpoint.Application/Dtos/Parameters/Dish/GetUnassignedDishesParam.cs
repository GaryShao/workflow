using System.ComponentModel.DataAnnotations;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters.Dish
{
    public class GetUnassignedDishesParam
    {
        public string RestaurantId { get; set; }

        [Required]
        public string MenuId { get; set; }
    }
}
