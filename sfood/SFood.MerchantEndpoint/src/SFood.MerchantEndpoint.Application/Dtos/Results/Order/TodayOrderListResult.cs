using Newtonsoft.Json;
using System.Collections.Generic;

namespace SFood.MerchantEndpoint.Application.Dtos.Results.Order
{
    public class TodayOrderListResult
    {
        public TodayOrderListResult()
        {
            Orders = new List<OrderRoughResult>();
        }

        [JsonProperty("orderList")]
        public List<OrderRoughResult> Orders { get; set; }

        [JsonProperty("total")]
        public int TotalCount { get; set; }

        public bool IsLastPage { get; set; } = true;
    }
}
