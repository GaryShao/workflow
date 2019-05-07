using Newtonsoft.Json;
using System.Collections.Generic;

namespace SFood.MerchantEndpoint.Application.Dtos.Results.Statistic
{
    public class DishesStatisticResult
    {
        public DishesStatisticResult()
        {
            Dishes = new List<SingleDish_StatisticInfo>();
        }

        [JsonProperty("totalOrders")]
        public int CountOfOrder { get; set; } 

        [JsonProperty("totalSales")]
        public decimal Turnover { get; set; }

        public List<SingleDish_StatisticInfo> Dishes { get; set; }
    }

    public class SingleDish_StatisticInfo
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int SaleVolume { get; set; }

        [JsonProperty("sale")]
        public decimal Turnover { get; set; }
    }
}
