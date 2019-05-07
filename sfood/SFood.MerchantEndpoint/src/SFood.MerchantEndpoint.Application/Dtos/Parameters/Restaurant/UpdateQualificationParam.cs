using Newtonsoft.Json;
using SFood.DataAccess.Common.Enums;
using System.Collections.Generic;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters.Restaurant
{
    public class UpdateQualificationParam
    {
        public UpdateQualificationParam()
        {
            Certs = new List<CertDto>();
        }

        public string RestaurantId { get; set; }

        [JsonProperty("name")]
        public string RestaurantName { get; set; }

        [JsonProperty("location")]
        public string RestaurantNo { get; set; }

        [JsonProperty("categorys")]
        public List<string> Categories { get; set; }

        [JsonProperty("imageUrls")]
        public List<CertDto> Certs { get; set; }


        public class CertDto
        {
            [JsonProperty("imageType")]
            public MerchantQualificationEntry Type { get; set; }

            [JsonProperty("imageUrl")]
            public string Value { get; set; }
        }
    }
}
