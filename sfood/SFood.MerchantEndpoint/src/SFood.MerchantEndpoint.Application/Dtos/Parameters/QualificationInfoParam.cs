using Newtonsoft.Json;
using SFood.DataAccess.Common.Consts;
using SFood.DataAccess.Common.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters
{
    public class QualificationInfoParam
    {
        public string RestaurantId { get; set; }

        [Required]
        public MerchantApplicationType ApplicationType { get; set; }

        public CompanyInfo CompanyInfo { get; set; }

        public List<ImageInfo> ImageUrls { get; set; }
    }

    public class CompanyInfo
    {
        [JsonProperty("companyName")]
        [MaxLength(DbConst.Length_100)]
        public string Name { get; set; }

        [JsonProperty("companyAddress")]
        [MaxLength(DbConst.Length_500)]
        public string Address { get; set; }

        [JsonProperty("companyRange")]
        [MaxLength(DbConst.Length_100)]
        public string BusinessScope { get; set; }
    }

    public class ImageInfo
    {
        [JsonProperty("imageType")]        
        public MerchantQualificationEntry Type { get; set; }

        [JsonProperty("imageUrl")]
        public string Url { get; set; }
    }
}
