using Newtonsoft.Json;
using SFood.DataAccess.Common.Enums;
using System;
using System.Collections.Generic;

namespace SFood.MerchantEndpoint.Application.Dtos.Results.Order
{
    public class OrderRoughResult
    {
        [JsonProperty("orderId")]
        public string Id { get; set; }

        public string Note { get; set; }

        [JsonProperty("seatNo")]
        public string SeatName { get; set; }

        public OrderStatus Status { get; set; }

        [JsonProperty("mealType")]        
        public int IsDishPacked { get; set; }

        public DeliveryType DeliveryType { get; set; }

        [JsonProperty("orderTime")]
        public DateTime CreatedTime { get; set; }

        [JsonProperty("mealNumber")]
        public string FetchNumber { get; set; }

        [JsonProperty("orderTimeText")]
        public string CreatedTimeDescription { get; set; }

        public decimal TotalBill { get; set; }

        public List<OrderDishResult> Dishes { get; set; }

        public bool IsLastPage { get; set; }
    }
}
