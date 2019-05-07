using AutoMapper;
using Newtonsoft.Json;
using SFood.BusinessInfo.Application.Dtos.Intermediates;
using SFood.BusinessInfo.Application.Dtos.Parameters;
using SFood.BusinessInfo.Application.Dtos.Responses;
using SFood.BusinessInfo.Common.Exceptions;
using SFood.BusinessInfo.Common.Extensions;
using SFood.DataAccess.Infrastructure.Interfaces;
using SFood.DataAccess.Models;
using SFood.DataAccess.Models.RelationshipModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFood.BusinessInfo.Application.Implements
{
    public class OrderService : IOrderService
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public OrderService(IReadOnlyRepository readonlyRepository
            , IRepository repository
            , IMapper mapper)
        {
            _readOnlyRepository = readonlyRepository;
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// 获取用户的所有在处理订单（未结束订单）
        /// 目前看， 此查询不涉及大量数据，暂不做分页
        /// </summary>
        /// <param name="userId">用户的identifier</param>
        /// <returns></returns>
        public IEnumerable<RoughOrderDto> RetrieveDealingOrders(string userId)
        {
            var result = _readOnlyRepository.GetAll<DealingOrder>(orders => orders.OrderByDescending(o => o.CreatedTime)).
                    Where(o => o.UserId == userId).
                    Select(o => _mapper.Map<RoughOrderDto>(o));

            return result;
        }

        public IEnumerable<RoughOrderDto> RetrieveArchivedOrders(string userId, int take = 10, int skip = 0)
        {
            var result = _readOnlyRepository.GetAll<ArchivedOrder>(orders => orders.OrderByDescending(o => o.CreatedTime), null, skip, take).
                    Where(o => o.UserId == userId).
                    Select(o => _mapper.Map<RoughOrderDto>(o));
            return result;
        }

        public DetailedOrderDto RetrieveDetailInfo(string orderId)
        {
            if (orderId.IsNullOrWhiteSpace())
            {
                throw new BadRequestException("OrderId is Required!");
            }

            return null;
        }

        public DetailedOrderDto CreateOrder(CreateOrderParam param)
        {
            //1. 分配 fetch number                        
            //2. 查询 数据           
            //3. 插入到dealingorder表
            

        }

        private async Task<DetailedOrderDto> RetrieveDealingOrderDetailInfo(string orderId)
        {
            var order = await _readOnlyRepository.GetFirstAsync<DealingOrder>(o => o.Id == orderId, null, "Restaurant, Seat");
            var detailedOrder = _mapper.Map<DetailedOrderDto>(order);
            detailedOrder.RestaurantName = order.Restaurant.Name;
            detailedOrder.SeatName = order.Seat?.Name;
            detailedOrder.Dishes = JsonConvert.DeserializeObject<IEnumerable<OrderDishDto>>(order.Dishes);
            detailedOrder.AmountOfDishes = detailedOrder.Dishes.Sum(d => d.Amount);

            var totalExpense = default(decimal);
            var amountOfDishes = default(int);
            foreach (var dish in detailedOrder.Dishes)
            {
                var suitPrice = dish.UnitPrice;
                foreach (var customization in dish.Customizations)
                {
                    suitPrice += customization.UnitPrice;
                }
                totalExpense += suitPrice * dish.Amount;
                amountOfDishes += dish.Amount;
            }

            detailedOrder.TotalBill = totalExpense;
            detailedOrder.AmountOfDishes = amountOfDishes;

            return detailedOrder;
        }

        private async Task<DetailedOrderDto> RetrieveArchivedOrderDetailInfo(string orderId)
        {
            var result = new DetailedOrderDto();

            var orders = _readOnlyRepository.GetAll<ArchivedOrder>(null, "Restaurant, Seat").
                        Where(o => o.Id == orderId);

            var dishes = _readOnlyRepository.GetAll<Order_Dish>(null, "Dish").
                    Where(o => o.OrderId == orderId);

            var customizations = _readOnlyRepository.GetAll<OrderDish_Customization>(null, "Customization")
                .Where(odc => odc.OrderId == orderId);

            var orderDishes = from order in orders
                              join dish in dishes on order.Id equals dish.OrderId
                              select new
                              {
                                  OrderDishId = dish.Id, //unique key
                                  OrderId = order.Id,
                                  order.Status,
                                  RestaurantName = order.Restaurant.Name,
                                  SeatName = order.Seat?.Name,
                                  order.DeliveryType,
                                  order.Note,
                                  order.FetchNumber,
                                  order.CreatedTime,
                                  DishId = dish.Dish.Id,
                                  DishName = dish.Dish.Name,
                                  dish.DishUnitPrice,
                                  dish.Amount
                              };

            var intermediateQuery = (from orderDish in orderDishes
                                    join customization in customizations on orderDish.OrderDishId equals customization.OrderDishId
                                    group new { orderDish, customization } by orderDish
                        into temp
                                    select new OrderContentEntryDto
                                    {
                                        OrderDishId = temp.Key.OrderDishId,
                                        OrderId = temp.Key.OrderId,
                                        Status = temp.Key.Status,
                                        RestaurantName = temp.Key.RestaurantName,
                                        SeatName = temp.Key.SeatName,
                                        DeliveryType = temp.Key.DeliveryType,
                                        Note = temp.Key.Note,
                                        FetchNumber = temp.Key.FetchNumber,
                                        CreatedTime = temp.Key.CreatedTime,
                                        DishId = temp.Key.DishId,
                                        DishName = temp.Key.DishName,
                                        DishUnitPrice = temp.Key.DishUnitPrice,
                                        Amount = temp.Key.Amount,
                                        Customizations = temp.Select(t => _mapper.Map<OrderDishCustomizationDto>(t))
                                    }).ToList();
            var entry = intermediateQuery.First();

            result.Id = entry.OrderDishId;
            result.Status = entry.Status;
            result.CreatedTime = entry.CreatedTime;
            result.RestaurantName = entry.RestaurantName;
            result.DeliveryType = entry.DeliveryType;
            result.Note = entry.Note;
            result.SeatName = entry.SeatName;
            result.FetchNumber = entry.FetchNumber;

            result.TotalBill = 1;
            result.AmountOfDishes = intermediateQuery.Sum(iq => iq.Amount);
            result.Dishes = intermediateQuery.Select(iq => new OrderDishDto
            {
                Id = iq.DishId,
                Name = iq.DishName,
                UnitPrice = iq.DishUnitPrice,
                Amount = iq.Amount,
                Customizations = iq.Customizations
            });

            return result;
        }
    }
}
