using SFood.DataAccess.Common.Enums;
using System.Collections.Generic;

namespace SFood.BusinessInfo.Application.Dtos.Responses
{
    public class CategorisedRestaurantImagesDto
    {
        public CategorisedRestaurantImagesDto()
        {
            Images = new List<string>();
        }

        public RestaurantLogoCategory Category { get; set; }

        public IEnumerable<string> Images { get; set; }
    }
}
