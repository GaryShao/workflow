using SFood.DataAccess.Common.Enums;
using System.Collections.Generic;

namespace SFood.BusinessInfo.Application.Dtos.Parameters
{
    public class CreateOrderParam
    {
        public string CenterId { get; set; }

        public string SeatId { get; set; }

        public string RestaurantId { get; set; }

        public DeliveryType DeliveryType { get; set; }

        public bool IsDishPacked { get; set; }

        public string Note { get; set; }

        public List<CreateOrderDishParam> Dishes { get; set; }
    }

    public class CreateOrderDishParam
    {
        public string Id { get; set; }

        public int Amount { get; set; }

        public List<string> Customizations { get; set; }
    }
}
