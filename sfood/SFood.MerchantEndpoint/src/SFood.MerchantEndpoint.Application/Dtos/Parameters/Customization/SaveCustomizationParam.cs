using Newtonsoft.Json;
using System.Collections.Generic;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters.Customization
{
    public class SaveCustomizationParam
    {
        public SaveCustomizationParam()
        {
            Categories = new List<CategoryParam>();
        }

        public string DishId { get; set; }

        public bool IsNew { get; set; }

        public string RestaurantId { get; set; }

        [JsonProperty("specs")]
        public List<CategoryParam> Categories { get; set; }
    }
}
