using System.Collections.Generic;

namespace SFood.ClientEndpoint.Application.Dtos.Result
{
    public class CenterRoughDto
    {
        public CenterRoughDto()
        {
            Banners = new List<string>();
            Categories = new List<RestaurantCategoryResult>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string SeatName { get; set; }

        public IEnumerable<string> Banners { get; set; }

        public IEnumerable<RestaurantCategoryResult> Categories { get; set; }
    }
}
