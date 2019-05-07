using Newtonsoft.Json;
using System.Collections.Generic;

namespace SFood.ClientEndpoint.Application.Dtos.Internal
{
    public class OrderDishDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal UnitPrice { get; set; }

        public byte Amount { get; set; }

        [JsonProperty("customization")]
        public List<CustomizationDto> Customizations { get; set; }

        [JsonIgnore]
        public decimal SuitePrice { get; set; }

        [JsonIgnore]
        public string CustomizationContent { get; set; }

        public class CustomizationDto
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public decimal UnitPrice { get; set; }
        }
    }
}
