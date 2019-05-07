using Newtonsoft.Json;
using SFood.DataAccess.Common.Enums;
using System.Collections.Generic;

namespace SFood.MerchantEndpoint.Application.Dtos.Results.Restaurant
{
    public class QualificationInfoResult
    {
        [JsonProperty("shopState")]
        public int QualificationStatus { get; set; }

        [JsonProperty("shopName")]
        public NameEntryDto NameEntry { get; set; }

        [JsonProperty("shopCategory")]
        public CategoryEntryDto CategoryEntry { get; set; }

        [JsonProperty("shopLocation")]
        public LocationEntryDto LocationEntry { get; set; }

        [JsonProperty("shopImage")]
        public CertCollectionDto CertCollection { get; set; }


        public class NameEntryDto
        {
            public string Name { get; set; }

            [JsonProperty("state")]
            public int QualificationStatus { get; set; }

            [JsonProperty("errorReason")]
            public string Reason { get; set; }
        }

        public class LocationEntryDto
        {
            public string Name { get; set; }

            [JsonProperty("state")]
            public int QualificationStatus { get; set; }

            [JsonProperty("errorReason")]
            public string Reason { get; set; }
        }

        public class CertCollectionDto
        {
            public CertCollectionDto()
            {
                Certs = new List<CertDto>();
            }

            public int ApplicationType { get; set; }

            [JsonProperty("shoImageUrls")]
            public List<CertDto> Certs { get; set; }
        }

        public class CertDto
        {
            [JsonProperty("imageType")]
            public MerchantQualificationEntry Type { get; set; }

            [JsonProperty("imageUrl")]
            public string Value { get; set; }

            [JsonProperty("state")]
            public int Status { get; set; }

            [JsonProperty("errorReason")]
            public string Reason { get; set; }
        }

        public class CategoryEntryDto
        {
            public CategoryEntryDto()
            {
                Categories = new List<CategoryDto>();
            }

            [JsonProperty("categorys")]
            public List<CategoryDto> Categories { get; set; }

            [JsonProperty("state")]
            public int QualificationStatus { get; set; }

            [JsonProperty("errorReason")]
            public string Reason { get; set; }
        }

        public class CategoryDto
        {
            [JsonProperty("categoryId")]
            public string Id { get; set; }

            [JsonProperty("categoryName")]
            public string Name { get; set; }
        }
    }    
}
