using System.Collections.Generic;

namespace SFood.BackendService.Application.Dtos
{
    public class OrderDishResult
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal UnitPrice { get; set; }

        public byte Amount { get; set; }
        
        public decimal SuitePrice { get; set; }
        
        public string CustomizationContent { get; set; }

        public List<CustomizationDto> Customizations { get; set; }

        public class CustomizationDto
        {
            public string Name { get; set; }

            public decimal UnitPrice { get; set; }
        }
    }
}
