using System.Collections.Generic;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters.Customization
{
    public class AddCustomizationsParam
    {
        public AddCustomizationsParam()
        {
            Categories = new List<CategoryParam>();
        }

        public string RestaurantId { get; set; }

        public List<CategoryParam> Categories { get; set; }
    }   
}
