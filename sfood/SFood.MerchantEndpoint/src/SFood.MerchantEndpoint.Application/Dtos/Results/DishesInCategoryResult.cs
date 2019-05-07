using System.Collections.Generic;

namespace SFood.MerchantEndpoint.Application.Dtos.Results
{
    public class DishesInCategoryResult
    {
        public DishesInCategoryResult()
        {
            Dishes = new List<DishResult>();
        }

        public string CategoryId { get; set; }
        public List<DishResult> Dishes { get; set; }
    }

    public class DishResult
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public byte Index { get; set; }
        public string Icon { get; set; }
    }
}
