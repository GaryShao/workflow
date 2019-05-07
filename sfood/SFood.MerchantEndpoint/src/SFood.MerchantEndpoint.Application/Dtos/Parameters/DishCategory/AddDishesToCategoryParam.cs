using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters.DishCategory
{
    public class AddDishesToCategoryParam
    {
        [Required]
        public string CategoryId { get; set; }

        [Required]
        public List<string> DishIds { get; set; }

        public string RestaurantId { get; set; }
    }
}
