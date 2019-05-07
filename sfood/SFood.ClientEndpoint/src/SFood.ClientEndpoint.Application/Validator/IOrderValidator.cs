using SFood.ClientEndpoint.Application.Dtos.Internal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFood.ClientEndpoint.Application.Validator
{
    public interface IOrderValidator
    {
        Task ValidateOrderInfo(List<OrderDishDto> orderDishes, string restaurantId);
    }
}
