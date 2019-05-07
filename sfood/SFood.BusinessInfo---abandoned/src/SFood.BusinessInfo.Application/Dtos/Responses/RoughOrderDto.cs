using SFood.DataAccess.Common.Enums;
using System;

namespace SFood.BusinessInfo.Application.Dtos.Responses
{
    public class RoughOrderDto
    {
        public string Id { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string RestaurantName { get; set; }
        public DeliveryType DeliveryType { get; set; }
        public string Note { get; set; }
        public string SeatName { get; set; }
        public string TotalBill { get; set; }
        public string FetchNumber { get; set; }
    }
}
