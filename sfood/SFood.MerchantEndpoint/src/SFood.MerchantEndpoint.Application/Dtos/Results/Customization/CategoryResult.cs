using Newtonsoft.Json;
using System.Collections.Generic;

namespace SFood.MerchantEndpoint.Application.Dtos.Results.Customization
{
    public class CategoryResult
    {
        [JsonProperty("specId")]
        public string Id { get; set; }

        [JsonProperty("specName")]
        public string Name { get; set; }

        [JsonProperty("isPreset")]
        public bool IsSystem { get; set; }

        [JsonProperty("maxSelectedNum")]
        public byte MaxSelected { get; set; }

        [JsonProperty("isMultiSelect")]
        public bool IsMultiple { get; set; }

        public bool IsSelected { get; set; }

        [JsonProperty("selections")]
        public List<OptionResult> Options { get; set; }
    }
}
