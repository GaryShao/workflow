using Newtonsoft.Json;
using SFood.DataAccess.Common.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters
{
    public class RestaurantProfileParam
    {
        public RestaurantProfileParam()
        {
            CategoryIds = new List<string>();
            CategoriedImages = new List<CategoriedImages>();
        }

        public string RestaurantId { get; set; }

        [JsonProperty("notice")]
        [Required]
        public string Announcement { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }

        [JsonProperty("location")]
        public string RestaurantNo { get; set; }

        [JsonProperty("countryId")]
        public string CountryCodeId { get; set; }

        public string Phone { get; set; }

        public string Logo { get; set; }

        [JsonProperty("brandIntroduction")]
        public string Introduction { get; set; }

        [JsonProperty("catalog")]
        public List<string> CategoryIds { get; set; }

        [JsonProperty("realSceneImage")]
        public List<CategoriedImages> CategoriedImages { get; set; }
    }

    public class CategoriedImages
    {
        public CategoriedImages()
        {
            Images = new List<string>();
        }

        [JsonProperty("imageType")]
        public RestaurantImageCategory Category { get; set; }

        [JsonProperty("imageUrl")]
        public List<string> Images { get; set; }
    }
}
