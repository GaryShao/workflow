using Newtonsoft.Json;

namespace SFood.MerchantEndpoint.Application.Dtos.Results.Statistic
{
    public class TodayStatisticInfoResult
    {
        [JsonProperty("orderCount")]
        public int CountOfOrder { get; set; }

        public decimal Turnover { get; set; }

        [JsonProperty("averagePrice")]
        public decimal AverageUnitPrice { get; set; }
    }
}
