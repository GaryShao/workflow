using Newtonsoft.Json;
using System;

namespace SFood.MerchantEndpoint.Application.Dtos.Results.Statistic
{
    public class StatisticInfoResult
    {
        public DateTime Date { get; set; }

        [JsonProperty("content")]
        public object Value { get; set; }
    }
}
