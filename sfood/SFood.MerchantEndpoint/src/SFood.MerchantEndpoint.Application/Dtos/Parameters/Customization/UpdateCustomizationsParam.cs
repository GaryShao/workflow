using Newtonsoft.Json;
using System.Collections.Generic;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters.Customization
{
    public class UpdateCustomizationsParam
    {
        public UpdateCustomizationsParam()
        {
            Categories = new List<CategoryParam>();
        }

        public string DishId { get; set; }

        public string RestaurantId { get; set; }

        public List<CategoryParam> Categories { get; set; }
    }   
}
