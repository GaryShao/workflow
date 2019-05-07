using Newtonsoft.Json;
using System.Collections.Generic;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters.Customization
{
    /// <summary>
    /// 规格的分类
    /// </summary>
    public class CategoryParam
    {
        public CategoryParam()
        {
            Options = new List<OptionParam>();
        }

        [JsonProperty("specId")]
        public string Id { get; set; }

        [JsonProperty("specName")]
        public string Name { get; set; }

        [JsonProperty("maxSelectedNum")]
        public byte MaxSelected { get; set; }

        [JsonProperty("isMultiSelect")]
        public bool IsMultiple { get; set; }

        [JsonProperty("isPreset")]
        public bool IsSystem { get; set; }

        public byte Index { get; set; }

        public string FromId { get; set; }

        public bool IsSelected { get; set; }

        [JsonProperty("selections")]
        public List<OptionParam> Options { get; set; }
    }
}
