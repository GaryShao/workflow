using Newtonsoft.Json;
using SFood.DataAccess.Common.Enums;
using System;
using System.Collections.Generic;

namespace SFood.MerchantEndpoint.Application.Dtos.Results.Order
{
    public class OrderDetailResult
    {
        [JsonProperty("orderId")]
        public string Id { get; set; }

        public string OrderNumber { get; set; }

        [JsonProperty("orderNote")]
        public string Note { get; set; }

        [JsonProperty("orderCreateTime")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("mealType")]
        public int IsPacked { get; set; }

        /// <summary>
        /// 切换到当前状态的时间
        /// </summary>
        [JsonProperty("orderStatusTime")]
        public DateTime? StatusTime { get; set; }
        
        public int? CountDown { get; set; }

        public string RestaurantName { get; set; }

        [JsonProperty("seatNo")]
        public string SeatName { get; set; }

        [JsonProperty("orderStatus")]
        public OrderStatus Status { get; set; }

        public DeliveryType DeliveryType { get; set; }

        [JsonProperty("payMethod")]
        public PaymentType PaymentType { get; set; }

        [JsonProperty("takeNo")]
        public string FetchNumber { get; set; }

        [JsonProperty("customerPhone")]
        public string Phone { get; set; }

        [JsonProperty("closeReason")]
        public string RefusedReason { get; set; }       

        public decimal TotalBill { get; set; }

        public byte AmountOfDishes { get; set; }

        public List<OrderDishResult> Dishes { get; set; }
    }
}
