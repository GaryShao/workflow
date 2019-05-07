using System.Collections.Generic;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters
{
    public class RestaurantImagesParam
    {
        public RestaurantImagesParam()
        {
            Categories = new List<CategoriedImages>();
        }

        public List<CategoriedImages> Categories { get; set; }

        public string RestaurantId { get; set; }
    }
}
