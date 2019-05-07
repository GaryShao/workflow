using SFood.MerchantEndpoint.Application.Dtos.Parameters.Statistic;
using SFood.MerchantEndpoint.Application.Dtos.Results.Statistic;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Application
{
    public interface IStatisticService
    {
        Task<DishesStatisticResult> GetDishesStatistic(DishStatisticInfoParam param);

        Task<TodayStatisticInfoResult> TodayStatisticInfo(string restaurantId);

        Task<List<StatisticInfoResult>> GetStatisticInfo(GetInfoInTimeRangeParam param);

        Task<IncomeDetailResult> GetPagedIncomeList(PagedIncomeListParam param);
    }
}
