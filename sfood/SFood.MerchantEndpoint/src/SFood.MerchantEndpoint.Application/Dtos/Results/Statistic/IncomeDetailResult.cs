using Newtonsoft.Json;
using SFood.DataAccess.Common.Enums;
using System;
using System.Collections.Generic;

namespace SFood.MerchantEndpoint.Application.Dtos.Results.Statistic
{
    public class IncomeDetailResult
    {
        public IncomeDetailResult()
        {
            Records = new List<Order_IncomeOneDay>();
        }
        
        [JsonProperty("totalOrderCount")]
        public int OrderCount { get; set; }

        [JsonProperty("totalTurnover")]
        public decimal Turnover { get; set; }

        [JsonProperty("records")]
        public List<Order_IncomeOneDay> Records { get; set; }
    }

    public class Order_IncomeOneDay
    {
        public Order_IncomeOneDay()
        {
            Orders = new List<Order_IncomeDetal>();
        }

        [JsonProperty("date")]
        public DateTime OrderCreatedTime { get; set; }

        public List<Order_IncomeDetal> Orders { get; set; }
    }

    public class Order_IncomeDetal
    {
        public string OrderId { get; set; }

        public string OrderCode { get; set; }

        [JsonProperty("date")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("payment")]
        public PaymentType PaymentType { get; set; }

        [JsonProperty("status")]
        public bool IsSuccessful { get; set; }

        public decimal Total { get; set; }
    }
}
