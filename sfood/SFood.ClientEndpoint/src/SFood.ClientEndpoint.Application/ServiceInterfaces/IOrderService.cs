using SFood.ClientEndpoint.Application.Dtos.Internal;
using SFood.ClientEndpoint.Application.Dtos.Parameters.Order;
using SFood.ClientEndpoint.Application.Dtos.Result.Orders;
using System.Threading.Tasks;

namespace SFood.ClientEndpoint.Application.ServiceInterfaces
{
    public interface IOrderService
    {
        Task<string> PlaceOrder(PlaceOrderParam param);

        Task<PagedList<OrderRoughResult>> GetAllDealing(string userId, int pageIndex, int pageSize);

        Task<PagedList<OrderRoughResult>> GetAll(string userId, int pageIndex, int pageSize);

        Task<OrderDetailResult> GetDetail(string orderId);

        Task Cancel(string orderId, string refusedReason, string userId);
    }
}
