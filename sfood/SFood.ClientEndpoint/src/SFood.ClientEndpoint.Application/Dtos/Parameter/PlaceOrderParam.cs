using SFood.ClientEndpoint.Application.Dtos.Internal;
using SFood.DataAccess.Common.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SFood.ClientEndpoint.Application.Dtos.Parameters.Order
{
    public class PlaceOrderParam
    {
        [Required]
        public string CenterId { get; set; }

        public string SeatId { get; set; }

        public string RestaurantId { get; set; }

        public string UserId { get; set; }

        [Required]
        public DeliveryType DeliveryType { get; set; }

        [Required]
        public PaymentType PaymentType { get; set; }

        public string ContactPhone { get; set; }

        [Required]
        public bool IsDishPacked { get; set; }

        public string Note { get; set; }

        [Required]
        public List<OrderDishDto> Dishes { get; set; }
    }
}
