using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SFood.DataAccess.Common.Enums;
using SFood.DataAccess.EFCore;
using SFood.DataAccess.Infrastructure.Interfaces;
using SFood.DataAccess.Models;
using SFood.DataAccess.Models.ProcedureModels;
using SFood.MerchantEndpoint.Application.Dtos.Parameters.Statistic;
using SFood.MerchantEndpoint.Application.Dtos.Results.Statistic;
using SFood.MerchantEndpoint.Common.Comparers;
using SFood.MerchantEndpoint.Common.Enums;
using SFood.MerchantEndpoint.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Application.ServiceImplements
{
    public class StatisticService : IStatisticService
    {
        private readonly IRepository _repository;
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IMapper _mapper;
        private readonly SFoodDbContext _dbContext;

        public StatisticService(IRepository repository,
            IReadOnlyRepository readOnlyRepository,
            IMapper mapper,
            SFoodDbContext dbContext)
        {
            _repository = repository;
            _readOnlyRepository = readOnlyRepository;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task<DishesStatisticResult> GetDishesStatistic(DishStatisticInfoParam param)
        {
            var result = new DishesStatisticResult();

            var allDishes = (await _readOnlyRepository.GetAllAsync<Dish>(d =>
                d.RestaurantId == param.RestaurantId)).Select(d => new SingleDish_StatisticInfo {
                    Id = d.Id,
                    Name = d.Name
                }).ToList();

            var sqlParams = new SqlParameter[3] {
                new SqlParameter("@RestaurantId", param.RestaurantId),
                new SqlParameter("@BeginTime", param.BeginTime),
                new SqlParameter("@EndTime", param.EndTime.AddDays(1).AddSeconds(-1))
            };

            var dishStatistics = _dbContext.USP_DishStatistics.
                FromSql("[Statistic].[uspGetDishesInfo] @RestaurantId, @BeginTime, @EndTime", sqlParams).ToList();

            var comparer = new DishStatisticComparer();

            foreach (var dish in allDishes)
            {
                dish.Turnover = GetTurnover(dish.Id, dishStatistics, comparer);
                dish.SaleVolume = GetSaleVolume(dish.Id, dishStatistics, comparer);
            }

            if (param.OrderBy == StatisticDimension.Turnover)
            {
                if (param.IsAsc)
                {
                    result.Dishes = allDishes.OrderBy(d => d.Turnover).ToList();
                }
                else
                {
                    result.Dishes = allDishes.OrderByDescending(d => d.Turnover).ToList();
                }                
            }

            if (param.OrderBy == StatisticDimension.SaleVolume)
            {
                if (param.IsAsc)
                {
                    result.Dishes = allDishes.OrderBy(d => d.SaleVolume).ToList();
                }
                else
                {
                    result.Dishes = allDishes.OrderByDescending(d => d.SaleVolume).ToList();
                }
            }

            result.CountOfOrder = await GetOrderCount(new GetInfoInTimeRangeParam {
                BeginTime = param.BeginTime,
                EndTime = param.EndTime,
                RestaurantId = param.RestaurantId
            });
            result.Turnover = await GetTurnover(new GetInfoInTimeRangeParam
            {
                BeginTime = param.BeginTime,
                EndTime = param.EndTime,
                RestaurantId = param.RestaurantId
            });
            return result;
        }         

        public async Task<TodayStatisticInfoResult> TodayStatisticInfo(string restaurantId)
        {
            var result = new TodayStatisticInfoResult();

            var todayOrders = (await _readOnlyRepository.GetAllAsync<ArchivedOrder>(o =>
                o.RestaurantId == restaurantId &&
                o.Status == OrderStatus.Done &&
                o.CreatedTime.Date == DateTime.UtcNow.Date, null, "Bill")).ToList();

            if (todayOrders == null || !todayOrders.Any())
            {
                return result;
            }

            result.CountOfOrder = todayOrders.Count();
            result.Turnover = todayOrders.Sum(o => o.Bill?.Amount ?? default(decimal));
            result.AverageUnitPrice = todayOrders.Average(o => o.Bill?.Amount ?? default(decimal));

            return result;
        }

        public async Task<List<StatisticInfoResult>> GetStatisticInfo(GetInfoInTimeRangeParam param)
        {
            switch (param.StatisticDimension)
            {
                case StatisticDimension.OrderCount:
                    return await GetOrderCountInTimeRange(param);
                case StatisticDimension.Turnover:
                    return await GetTurnoverInTimeRange(param);
                case StatisticDimension.AverageUnitPrice:
                    return await GetAvePriceInTimeRange(param);
                default:
                    throw new BadRequestException($"No such type configed in our system. ");
            }
        }

        public async Task<IncomeDetailResult> GetPagedIncomeList(PagedIncomeListParam param)
        {
            var result = new IncomeDetailResult();

            var count = await _readOnlyRepository.GetCountAsync<ArchivedOrder>(order =>
                order.Status == OrderStatus.Done &&
                order.CreatedTime >= param.BeginTime &&
                order.CreatedTime < param.EndTime.AddDays(1) &&
                order.RestaurantId == param.RestaurantId);

            var orders = (await _readOnlyRepository.GetAllAsync<ArchivedOrder>(order =>
                order.Status == OrderStatus.Done &&
                order.CreatedTime >= param.BeginTime &&
                order.CreatedTime < param.EndTime.AddDays(1) &&
                order.RestaurantId == param.RestaurantId, order => 
                order.OrderByDescending(o => o.CreatedTime), 
                "Bill", param.PageIndex * param.PageSize, param.PageSize)).ToList();

            var orderRecords = orders.Select(o => new Order_IncomeDetal
            {
                OrderId = o.Id,
                OrderCode = o.OrderNumber,
                CreatedAt = o.CreatedTime,
                PaymentType = o.PaymentType,
                IsSuccessful = o.Status == OrderStatus.Done,
                Total = o.Bill?.Amount ?? default(decimal)
            }).ToList();

            var records = orderRecords.GroupBy(o => o.CreatedAt.Date).Select(g => new Order_IncomeOneDay {
                OrderCreatedTime = g.Key,
                Orders = g.ToList()
            }).ToList();

            result.Records = records;
            result.OrderCount = count;
            result.Turnover = orderRecords.Sum(or => or.Total);
            return result;
        }


        /// <summary>
        /// get the order count info for every single day 
        /// during that timerange
        /// </summary>
        /// <returns></returns>
        private async Task<List<StatisticInfoResult>> GetOrderCountInTimeRange(GetInfoInTimeRangeParam param)
        {
            var orders = await _readOnlyRepository.GetAllAsync<ArchivedOrder>(o =>
                o.RestaurantId == param.RestaurantId &&
                o.Status == OrderStatus.Done &&
                o.CreatedTime >= param.BeginTime &&
                o.CreatedTime <= param.EndTime.AddDays(1).AddSeconds(-1), o => o.OrderBy(ao => ao.CreatedTime));

            var daysWithData = orders.ToList().GroupBy(o =>
                o.CreatedTime.Date).Select(gp => new StatisticInfoResult
                {
                    Date = gp.Key,
                    Value = gp.Count()
                }).ToList();

            var result = new List<StatisticInfoResult>();

            for (DateTime day = param.BeginTime.Date; day <= param.EndTime.Date ; day = day.AddDays(1))
            {
                var dayWithData = daysWithData.FirstOrDefault(d => d.Date == day);
                if (dayWithData != null)
                {
                    result.Add(dayWithData);
                }
                else
                {
                    result.Add(new StatisticInfoResult
                    {
                        Date = day,
                        Value = 0
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// get the turnover info for every single day 
        /// during that timerange
        /// </summary>
        /// <returns></returns>
        private async Task<List<StatisticInfoResult>> GetTurnoverInTimeRange(GetInfoInTimeRangeParam param)
        {
            var orders = await _readOnlyRepository.GetAllAsync<ArchivedOrder>(ao =>
                ao.RestaurantId == param.RestaurantId &&
                ao.Status == OrderStatus.Done &&
                ao.CreatedTime >= param.BeginTime &&
                ao.CreatedTime <= param.EndTime.AddDays(1).AddSeconds(-1), o => o.OrderBy(ao => ao.CreatedTime), "Bill");

            var daysWithData = orders.ToList().GroupBy(o =>
                o.CreatedTime.Date).Select(gp => new StatisticInfoResult
                {
                    Date = gp.Key,
                    Value = gp.Sum(order => order.Bill?.Amount)
                }).ToList();

            var result = new List<StatisticInfoResult>();

            for (DateTime day = param.BeginTime.Date; day <= param.EndTime.Date; day = day.AddDays(1))
            {
                var dayWithData = daysWithData.FirstOrDefault(d => d.Date == day);
                if (dayWithData != null)
                {
                    result.Add(dayWithData);
                }
                else
                {
                    result.Add(new StatisticInfoResult
                    {
                        Date = day,
                        Value = 0
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// get the average price of each order for every single day
        /// during that timerange
        /// </summary>
        /// <returns></returns>
        private async Task<List<StatisticInfoResult>> GetAvePriceInTimeRange(GetInfoInTimeRangeParam param)
        {
            var orders = await _readOnlyRepository.GetAllAsync<ArchivedOrder>(ao =>
                ao.RestaurantId == param.RestaurantId &&
                ao.Status == OrderStatus.Done &&
                ao.CreatedTime >= param.BeginTime &&
                ao.CreatedTime <= param.EndTime.AddDays(1).AddSeconds(-1), o => o.OrderBy(ao => ao.CreatedTime), "Bill");

            var daysWithData = orders.ToList().GroupBy(o =>
                o.CreatedTime.Date).Select(gp => new StatisticInfoResult
                {
                    Date = gp.Key,
                    Value = gp.Average(order => order.Bill?.Amount)
                }).ToList();

            var result = new List<StatisticInfoResult>();

            for (DateTime day = param.BeginTime.Date; day <= param.EndTime.Date; day = day.AddDays(1))
            {
                var dayWithData = daysWithData.FirstOrDefault(d => d.Date == day);
                if (dayWithData != null)
                {
                    result.Add(dayWithData);
                }
                else
                {
                    result.Add(new StatisticInfoResult
                    {
                        Date = day,
                        Value = 0
                    });
                }
            }

            return result;
        }

        private async Task<int> GetOrderCount(GetInfoInTimeRangeParam param)
        {
            var count = await _readOnlyRepository.GetCountAsync<ArchivedOrder>(o =>
                o.RestaurantId == param.RestaurantId &&
                o.Status == OrderStatus.Done &&
                o.CreatedTime >= param.BeginTime &&
                o.CreatedTime <= param.EndTime);
            return count;
        }

        private async Task<decimal> GetTurnover(GetInfoInTimeRangeParam param)
        {
            var orders = await _readOnlyRepository.GetAllAsync<ArchivedOrder>(ao =>
                ao.RestaurantId == param.RestaurantId &&
                ao.Status == OrderStatus.Done &&
                ao.CreatedTime >= param.BeginTime &&
                ao.CreatedTime <= param.EndTime, null, "Bill");

            return orders.Sum(o => o.Bill?.Amount ?? default(decimal));
        }

        private decimal GetTurnover(string dishId, List<USP_DishStatistic> dishStatistics, DishStatisticComparer comparer)
        {
            var dishPrice = dishStatistics.Where(ds => 
                    ds.DishId == dishId).Distinct(comparer).Sum(ds => ds.DishUnitPrice * ds.Amount);

            var customizationPrice = dishStatistics.Where(ds =>
                    ds.DishId == dishId).Sum(ds => ds.CustomizationUnitPrice * ds.Amount);

            return dishPrice + customizationPrice ?? default(decimal);
        }

        private int GetSaleVolume(string dishId, List<USP_DishStatistic> dishStatistics, DishStatisticComparer comparer)
        {
            var sum = dishStatistics.Where(ds =>
                ds.DishId == dishId).Distinct(comparer).Sum(ds => ds.Amount);
            return sum;
        }
    }
}
