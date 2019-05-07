using Newtonsoft.Json;
using SFood.DataAccess.Common.Enums;
using System.Collections.Generic;

namespace SFood.MerchantEndpoint.Application.Dtos.Results
{
    public class RestaurantProfileResult
    {
        public RestaurantProfileResult()
        {
            Images = new List<CategoriedImagesResult>();
        }

        [JsonProperty("notice")]
        public string Announcement { get; set; }

        [JsonProperty("baseInfo")]
        public RestaurantBasicInfoResult BasicInfo { get; set; }

        [JsonProperty("realSceneImage")]
        public List<CategoriedImagesResult> Images { get; set; }

        [JsonProperty("brandIntroduction")]
        public string Introduction { get; set; }
    }

    public class CategoryResult
    {
        [JsonProperty("categoryId")]
        public string Id { get; set; }

        [JsonProperty("categoryName")]
        public string Name { get; set; }
    }

    public class CategoriedImagesResult
    {
        public CategoriedImagesResult()
        {
            Images = new List<string>();
        }

        [JsonProperty("imageType")]
        public RestaurantImageCategory Category { get; set; }

        [JsonProperty("imageUrl")]
        public List<string> Images { get; set; }
    }

    public class RestaurantBasicInfoResult
    {
        public RestaurantBasicInfoResult()
        {
            Categories = new List<CategoryResult>();
        }

        public string Name { get; set; }

        [JsonProperty("imageUrl")]
        public string Logo { get; set; }

        [JsonProperty("openTime")]
        public short? OpenedAt { get; set; }

        [JsonProperty("closeTime")]
        public short? ClosedAt { get; set; }

        [JsonProperty("location")]
        public string RestaurantNo { get; set; }
        
        public CountryDto Country { get; set; }

        public string Phone { get; set; }

        [JsonProperty("supportDesk")]
        public bool IsDeliverySupport { get; set; }

        [JsonProperty("foodPavilionId")]
        public string HawkerCenterId { get; set; }

        [JsonProperty("foodPavilion")]
        public string HawkerCenterName { get; set; }

        public List<CategoryResult> Categories { get; set; }
    }

    public class CountryDto
    {
        [JsonProperty("countryId")]
        public string Id { get; set; }

        [JsonProperty("countryName")]
        public string Name { get; set; }

        [JsonProperty("countryCode")]
        public string Code { get; set; }

        [JsonProperty("countryFlagUrl")]
        public string FlagUrl { get; set; }
    }
}
