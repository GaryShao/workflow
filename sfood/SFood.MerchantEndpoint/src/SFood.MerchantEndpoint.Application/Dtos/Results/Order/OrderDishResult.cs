using Newtonsoft.Json;
using SFood.MerchantEndpoint.Application.Dtos.Internal;
using System.Collections.Generic;

namespace SFood.MerchantEndpoint.Application.Dtos.Results.Order
{
    public class OrderDishResult
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal UnitPrice { get; set; }

        public byte Amount { get; set; }

        [JsonProperty("totalPrice")]
        public decimal SuitePrice { get; set; }

        [JsonProperty("specificationsName")]
        public string CustomizationContent { get; set; }

        public List<OrderCustomizationDto> Customizations { get; set; }
    }
}
