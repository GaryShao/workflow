using Newtonsoft.Json;
using SFood.DataAccess.Common.Enums;
using System.Collections.Generic;

namespace SFood.ClientEndpoint.Application.Dtos.Result
{
    public class RestaurantProfileResult
    {
        public RestaurantProfileResult()
        {
            Images = new List<ImagesDto>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public string Logo { get; set; }

        public bool IsOpened { get; set; }

        public bool IsDeliverySupport { get; set; }

        public short OpenedAt { get; set; }

        public short ClosedAt { get; set; }

        public string RestaurantNo { get; set; }

        public string Phone { get; set; }

        public string Introduction { get; set; }

        public string CenterName { get; set; }

        public bool IsReceivingAuto { get; set; }

        public List<ImagesDto> Images { get; set; }

        public class ImagesDto
        {
            public ImagesDto()
            {
                Images = new List<string>();
            }

            public string Category { get; set; }

            [JsonProperty("urls")]
            public IEnumerable<string> Images { get; set; }
        }
    }
}
