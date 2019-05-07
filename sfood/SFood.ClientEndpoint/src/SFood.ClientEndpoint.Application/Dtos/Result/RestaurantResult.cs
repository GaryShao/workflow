using System.Collections.Generic;

namespace SFood.ClientEndpoint.Application.Dtos.Result
{
    public class RestaurantResult
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Logo { get; set; }

        public string Announcement { get; set; }

        public bool IsOpened { get; set; }

        public bool IsDeliverySupport { get; set; }

        public List<string> Categories { get; set; }
    }
}
