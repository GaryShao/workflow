using System;
using System.Collections.Generic;
using System.Text;

namespace SFood.BusinessInfo.Application.Dtos.Responses
{
    public class CenterRoughInfoDto
    {
        public CenterRoughInfoDto()
        {
            Categories = new List<RestaurantCategoryDto>();
        }

        public string CenterId { get; set; }
        public string Name { get; set; }
        public string SeatName { get; set; }
        public IEnumerable<string> Images { get; set; }
        public IEnumerable<RestaurantCategoryDto> Categories { get; set; }
    }
}
