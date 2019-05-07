using System.Collections.Generic;

namespace SFood.ClientEndpoint.Application.Dtos.Result
{
    public class MenuContentResult
    {
        public MenuContentResult()
        {
            Categories = new List<CategoryDto>();
        }

        public List<CategoryDto> Categories { get; set; }
      
        public class CategoryDto
        {
            public CategoryDto()
            {
                Dishes = new List<DishDto>();
            }

            public string Id { get; set; }

            public string Name { get; set; }

            public int Index { get; set; }

            public IEnumerable<DishDto> Dishes { get; set; }
        }

        public class DishDto
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public byte Index { get; set; }

            public string Logo { get; set; }

            public decimal UnitPrice { get; set; }

            public int SaleVolume { get; set; }

            public bool HasCustomization { get; set; }
        }
    }
}
