using SFood.DataAccess.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters.Order
{
    public class ChangeOrderStatusParam
    {
        [Required]
        public OrderStatus FromStatus { get; set; }

        [Required]
        public OrderStatus ToStatus { get; set; }

        [Required]
        public string OrderId { get; set; }

        public string RefusedReason { get; set; }

        public string RestaurantId { get; set; }
    }
}
