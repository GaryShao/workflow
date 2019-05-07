using System.Collections.Generic;

namespace SFood.BusinessInfo.Application.Dtos.Responses
{
    public class OrderDishDto
    {
        public OrderDishDto()
        {
            Customizations = new List<OrderDishCustomizationDto>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public int Amount { get; set; }
        public IEnumerable<OrderDishCustomizationDto> Customizations { get; set; }
    }
}
