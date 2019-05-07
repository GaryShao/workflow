using System;
using System.Collections.Generic;
using SFood.BusinessInfo.Application.Dtos.Responses;
using SFood.DataAccess.Common.Enums;

namespace SFood.BusinessInfo.Application.Dtos.Intermediates
{
    public class OrderContentEntryDto
    {
        public string OrderDishId { get; set; }
        public string OrderId { get; set; }
        public OrderStatus Status { get; set; }
        public string RestaurantName { get; set; }
        public string SeatName { get; set; }
        public DeliveryType DeliveryType { get; set; }
        public string Note { get; set; }
        public string FetchNumber { get; set; }
        public DateTime CreatedTime { get; set; }
        public string DishId { get; set; }
        public string DishName { get; set; }
        public decimal DishUnitPrice { get; set; }
        public int Amount { get; set; }
        public IEnumerable<OrderDishCustomizationDto> Customizations { get; set; }
    }
}