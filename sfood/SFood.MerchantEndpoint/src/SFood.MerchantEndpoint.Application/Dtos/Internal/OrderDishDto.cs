using System.Collections.Generic;

namespace SFood.MerchantEndpoint.Application.Dtos.Internal
{
    public class OrderDishDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal UnitPrice { get; set; }

        public byte Amount { get; set; }

        public List<OrderCustomizationDto> Customizations { get; set; }           
    }
}
