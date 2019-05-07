using SFood.DataAccess.Common.Enums;
using SFood.MerchantEndpoint.Application.Dtos.Parameters.Order;
using SFood.MerchantEndpoint.Application.Dtos.Results.Order;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Application
{
    public interface IOrderService
    {
        Task<OrderDetailResult> GetDetail(string orderId, string restaurantId);

        Task<int> GetTodayCount(string restaurantId);

        Task ChangeOrderStatus(ChangeOrderStatusParam param);

        Task<TodayOrderListResult> GetTodayList(OrderStatus status, bool isPaged, string restaurantId, int pageIndex, int pageSize);

        Task<AllOrderListResult> GetAllList(int status, string restaurantId, int pageIndex, int pageSize);
    }
}
