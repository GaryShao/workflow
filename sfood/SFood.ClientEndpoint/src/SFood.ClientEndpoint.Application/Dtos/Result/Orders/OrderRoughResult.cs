using SFood.DataAccess.Common.Enums;
using System;
using System.Collections.Generic;

namespace SFood.ClientEndpoint.Application.Dtos.Result.Orders
{
    public class OrderRoughResult
    {
        public string Id { get; set; }

        public string RestaurantName { get; set; }

        public string SeatNo { get; set; }

        public int? WaitingCustomerCount { get; set; }

        public OrderStatus Status { get; set; }

        public bool IsDishPacked { get; set; }

        public DeliveryType DeliveryType { get; set; }

        public DateTime CreatedTime { get; set; }

        public string FetchNumber { get; set; }

        public decimal TotalBill { get; set; }

        public List<string> Dishes { get; set; }
    }
}
