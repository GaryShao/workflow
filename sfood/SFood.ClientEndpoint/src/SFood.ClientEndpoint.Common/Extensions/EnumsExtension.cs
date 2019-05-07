using SFood.DataAccess.Common.Enums;

namespace SFood.ClientEndpoint.Common.Extensions
{
    public static class EnumsExtension
    {
        public static bool IsDealingOrder(this OrderStatus status)
        {
            return status == OrderStatus.Pending ||
                status == OrderStatus.Cooking ||
                status == OrderStatus.DeliveringOrTaking;
        }
    }
}
