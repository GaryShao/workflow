using Newtonsoft.Json;
using SFood.DataAccess.Common.Enums;
using System;
using System.Collections.Generic;

namespace SFood.BusinessInfo.Application.Dtos.Responses
{
    public class DetailedOrderDto
    {
        public DetailedOrderDto()
        {
            Dishes = new List<OrderDishDto>();
        }

        public string Id { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedTime { get; set; }
        public string RestaurantName { get; set; }
        public DeliveryType DeliveryType { get; set; }
        public string Note { get; set; }
        public string SeatName { get; set; }
        public decimal TotalBill { get; set; }
        public int AmountOfDishes { get; set; }
        public string FetchNumber { get; set; }
        [JsonIgnore]
        public IEnumerable<OrderDishDto> Dishes { get; set; }
    }    
}
