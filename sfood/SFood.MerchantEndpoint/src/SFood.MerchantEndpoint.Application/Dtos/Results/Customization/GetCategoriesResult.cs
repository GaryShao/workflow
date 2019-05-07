using Newtonsoft.Json;
using System.Collections.Generic;

namespace SFood.MerchantEndpoint.Application.Dtos.Results.Customization
{
    public class GetCategoriesResult
    {
        public string DishId { get; set; }

        public bool IsNew { get; set; }

        [JsonProperty("specs")]
        public List<CategoryResult> Categories { get; set; }
    }
}
