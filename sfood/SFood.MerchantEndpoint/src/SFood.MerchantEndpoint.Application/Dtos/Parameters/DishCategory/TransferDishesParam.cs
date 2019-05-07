using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters.DishCategory
{
    public class TransferDishesParam
    {
        public string RestaurantId { get; set; }

        [Required]
        public string MenuId { get; set; }

        [Required]
        public string ToCategoryId { get; set; }

        [Required]
        public List<string> DishIds { get; set; }
    }
}
