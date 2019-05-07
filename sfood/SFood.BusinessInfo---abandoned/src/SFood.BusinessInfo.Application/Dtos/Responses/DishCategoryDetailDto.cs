using System.Collections.Generic;

namespace SFood.BusinessInfo.Application.Dtos.Responses
{
    public class DishCategoryDetailDto
    {
        public DishCategoryDetailDto()
        {
            Dishes = new List<DishDto>();
        }

        public string CategoryId { get; set; }
        public string Name { get; set; }
        public IEnumerable<DishDto> Dishes { get; set; }
    }
}
