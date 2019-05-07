using Newtonsoft.Json;

namespace SFood.ClientEndpoint.Application.Dtos.Internal
{
    public class CountryCodeDto
    {
        [JsonProperty("countryId")]
        public string Id { get; set; }

        [JsonProperty("countryCode")]
        public string Code { get; set; }

        [JsonProperty("countryName")]
        public string Name { get; set; }

        public string EnglishName { get; set; }

        [JsonProperty("countryFlagUrl")]
        public string FlagUrl { get; set; }

        /// <summary>
        /// 汉语拼音
        /// </summary>
        [JsonIgnore]
        public string PinYin { get; set; }

        /// <summary>
        /// 汉语拼音的首字母
        /// </summary>
        [JsonIgnore]
        public string Initial { get; set; }
    }
}
