using System.Collections.Generic;

namespace SFood.BusinessInfo.Application.Dtos.Responses
{
    public class RestaurantProfileDto
    {
        public RestaurantProfileDto()
        {
            Images = new List<CategorisedRestaurantImagesDto>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public bool IsOpened { get; set; }
        public bool IsDeliverySupport { get; set; }
        public string OpenedAt { get; set; }
        public string ClosedAt { get; set; }
        public string RestaurantNo { get; set; }
        public string Phone { get; set; }
        public string Introduction { get; set; }
        public string CenterName { get; set; }
        public bool IsShowByTime { get; set; }
        public bool IsReceivingAuto { get; set; }

        public IEnumerable<CategorisedRestaurantImagesDto> Images { get; set; }
    }
}
