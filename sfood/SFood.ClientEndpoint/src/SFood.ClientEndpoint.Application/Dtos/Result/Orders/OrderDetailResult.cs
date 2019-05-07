using SFood.ClientEndpoint.Application.Dtos.Internal;
using SFood.DataAccess.Common.Enums;
using System;
using System.Collections.Generic;

namespace SFood.ClientEndpoint.Application.Dtos.Result.Orders
{
    public class OrderDetailResult
    {
        public string Id { get; set; }

        public string OrderNumber { get; set; }

        public string Note { get; set; }

        public DateTime CreatedAt { get; set; }

        public int IsPacked { get; set; }

        /// <summary>
        /// 切换到当前状态的时间
        /// </summary>        
        public DateTime? StatusTime { get; set; }

        public int? CountDown { get; set; }

        public string RestaurantName { get; set; }

        public string SeatName { get; set; }

        public OrderStatus Status { get; set; }

        public DeliveryType DeliveryType { get; set; }

        public PaymentType PaymentType { get; set; }

        public string FetchNumber { get; set; }

        public string Phone { get; set; }

        public string RefusedReason { get; set; }

        public decimal TotalBill { get; set; }

        public byte AmountOfDishes { get; set; }

        public List<OrderDishDto> Dishes { get; set; }
    }
}
