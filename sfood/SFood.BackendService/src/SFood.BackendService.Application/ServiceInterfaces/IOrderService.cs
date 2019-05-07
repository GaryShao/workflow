using System.Threading.Tasks;

namespace SFood.BackendService.Application.ServiceInterfaces
{
    public interface IOrderService
    {
        Task CancelOrdersAsync();

        Task TransferClosedOrders();
    }
}
