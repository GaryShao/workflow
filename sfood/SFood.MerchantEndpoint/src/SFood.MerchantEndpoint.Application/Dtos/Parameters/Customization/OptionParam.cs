using Newtonsoft.Json;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters.Customization
{
    /// <summary>
    /// 规格分类下的选项值
    /// </summary>
    public class OptionParam
    {
        [JsonProperty("selectionId")]
        public string Id { get; set; }

        public string Name { get; set; }

        public byte Index { get; set; }

        public decimal UnitPrice { get; set; }

        public bool IsDefault { get; set; }
    }
}
