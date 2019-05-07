using SFood.DataAccess.Models.ProcedureModels;
using System.Collections.Generic;

namespace SFood.MerchantEndpoint.Common.Comparers
{
    public class DishStatisticComparer : IEqualityComparer<USP_DishStatistic>
    {
        /// <summary>
        /// 这两者是一个订单里的一个菜品
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(USP_DishStatistic x, USP_DishStatistic y)
        {
            return x.OrderDishId == y.OrderDishId;
        }

        public int GetHashCode(USP_DishStatistic obj)
        {
            return obj.OrderDishId.GetHashCode();
        }
    }
}
