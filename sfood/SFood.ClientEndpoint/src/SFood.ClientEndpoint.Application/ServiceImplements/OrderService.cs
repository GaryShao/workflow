using Newtonsoft.Json;
using SFood.ClientEndpoint.Application.Dtos.Internal;
using SFood.ClientEndpoint.Application.Dtos.Parameters.Order;
using SFood.ClientEndpoint.Application.Dtos.Result.Orders;
using SFood.ClientEndpoint.Application.ServiceInterfaces;
using SFood.ClientEndpoint.Application.Validator;
using SFood.ClientEndpoint.Common.Consts;
using SFood.ClientEndpoint.Common.Exceptions;
using SFood.ClientEndpoint.Common.Extensions;
using SFood.DataAccess.Common.Enums;
using SFood.DataAccess.Infrastructure.Interfaces;
using SFood.DataAccess.Models;
using SFood.DataAccess.Models.RelationshipModels;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFood.ClientEndpoint.Application.ServiceImplements
{
    public class OrderService : IOrderService
    {
        private readonly IReadOnlyRepository _readOnlyRepository;
        private readonly IRepository _repository;
        private readonly IDishValidator _dishValidator;
        private readonly IOrderValidator _orderValidator;
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public OrderService(IReadOnlyRepository readonlyRepository
            , IOrderValidator orderValidator
            , IDishValidator dishValidator
            , IRepository repository
            , IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _readOnlyRepository = readonlyRepository;
            _dishValidator = dishValidator;
            _orderValidator = orderValidator;
            _repository = repository;            
        }

        public async Task<string> PlaceOrder(PlaceOrderParam param)
        {
            var center = await _readOnlyRepository.GetFirstAsync<HawkerCenter>(c => c.Id == param.CenterId &&
                !c.IsDeleted);
            if (center == null)
            {
                throw new BadRequestException($"No such center found in db, center id : {param.CenterId}");
            }

            var restaurant = await _readOnlyRepository.GetFirstAsync<Restaurant>(r => r.Id == param.RestaurantId, null, "RestaurantDetail");

            if (restaurant == null)
            {
                throw new BadRequestException($"No such restaurant found in db, restaurant id : {param.RestaurantId}");
            }

            if (!param.SeatId.IsNullOrEmpty())
            {
                var seat = await _readOnlyRepository.GetFirstAsync<Seat>(s => s.Id == param.SeatId);

                if (seat == null)
                {
                    throw new BadRequestException($"No such seat found in db, seat id : {param.SeatId}");
                }
            }

            if (param.Dishes == null || !param.Dishes.Any())
            {
                throw new BadRequestException($"No dishes found in your order. ");
            }

            await _dishValidator.ValidateBadDishId(param.Dishes.Select(d => d.Id).ToList(), param.RestaurantId);

            //await _orderValidator.ValidateOrderInfo(param.Dishes, param.RestaurantId);

            var order = new DealingOrder {
                Id = Guid.NewGuid().ToUuidString(),
                CenterId = param.CenterId,
                UserId = param.UserId,
                OrderNumber = $"F{DateTime.UtcNow.Date.ToSimpleDate()}0{MaskOrderIndex(await UpdateAndFetchDailyOrderIndex())}",
                ContactPhone = param.ContactPhone,
                RestaurantId = param.RestaurantId,
                SeatId = param.SeatId,
                Note = param.Note,
                IsDishPacked = param.IsDishPacked,
                PaymentType = param.PaymentType,
                DeliveryType = param.DeliveryType,
                Status = OrderStatus.Pending,
                Dishes = JsonConvert.SerializeObject(param.Dishes),
                FetchNumber = $"#{await UpdateAndFetchNumberForRestaurant(param.RestaurantId)}"
            };

            if (restaurant.RestaurantDetail.IsReceivingAuto)
            {
                order.Status = OrderStatus.Cooking;
            }

            return (await _repository.CreateAsync(order)).Id;
        }

        public async Task<PagedList<OrderRoughResult>> GetAllDealing(string userId, int pageIndex, int pageSize)
        {
            var totalCount = await _readOnlyRepository.GetCountAsync<DealingOrder>(order =>
                    order.UserId == userId);

            if (totalCount == 0)
            {
                return new PagedList<OrderRoughResult>();
            }

            var result = new PagedList<OrderRoughResult>
            {
                Count = totalCount
            };

            var orders = (await _readOnlyRepository.GetAllAsync<DealingOrder>(order =>
                    order.UserId == userId, order =>
                    order.OrderByDescending(o => o.CreatedTime), "Restaurant,Seat",
                    pageIndex * pageSize, pageSize)).ToList();

            var orderList = new List<OrderRoughResult>();

            orders.ForEach(order => {

                var dishes = JsonConvert.DeserializeObject<List<OrderDishDto>>(order.Dishes);

                var roughOrder = new OrderRoughResult
                {
                    Id = order.Id,
                    RestaurantName = order.Restaurant.Name,
                    SeatNo = order.Seat?.Name,
                    Status = order.Status,
                    IsDishPacked = order.IsDishPacked,
                    DeliveryType = order.DeliveryType,
                    FetchNumber = order.FetchNumber,
                    CreatedTime = order.CreatedTime,
                    WaitingCustomerCount = 3,
                    Dishes = dishes.Select(d => d.Name).ToList()
                };
                roughOrder.TotalBill = GetTotal(dishes);
                dishes.ForEach(dish => {
                    dish.SuitePrice = GetSuitePrice(dish);
                });
                orderList.Add(roughOrder);
            });

            result.Entities = orderList;
            return result;
        }

        public async Task<PagedList<OrderRoughResult>> GetAll(string userId, int pageIndex, int pageSize)
        {
            var dealings = await GetAllDealing(userId, pageIndex, pageSize);
            var result = new PagedList<OrderRoughResult>();

            if (((pageIndex + 1) * pageSize) <= dealings.Count)
            {
                var archivedPart = await GetAllArchive(userId, 0, pageSize);
                //situation 1 : all entities are from dealing orders
                result.Count = dealings.Count + archivedPart.Count;
                result.Entities = dealings.Entities;
                return result;
            }
            else if (((pageIndex + 1) * pageSize) - dealings.Count <= pageSize)
            {
                //situation 2 : part entities are from dealing orders and the other are from archived orders
                var dealingPart = await GetAllDealing(userId, pageIndex, pageSize);
                var archivedPart = await GetAllArchive(userId, 0, pageSize - dealingPart.Count % pageSize);

                result.Count = dealings.Count + archivedPart.Count;
                result.Entities.AddRange(dealingPart.Entities);
                result.Entities.AddRange(archivedPart.Entities);

                return result;
            }
            else
            {
                //situation 3 : all entities are from archived orders
                var newPageIndex = (((pageIndex + 1) * pageSize) - dealings.Count) / pageSize - 1;
                var archivedPart = await GetAllArchive(userId, newPageIndex, pageSize);

                result.Count = dealings.Count + archivedPart.Count;
                result.Entities.AddRange(archivedPart.Entities);
                return result;
            }
        }
        
        public async Task<OrderDetailResult> GetDetail(string orderId)
        {
            var dealingOrder = await GetDealingOrderDetail(orderId);
            if (dealingOrder != null)
            {
                return dealingOrder;
            }
            var archivedOrder = await GetArchiveOrderDetail(orderId);
            if (archivedOrder != null)
            {
                return archivedOrder;
            }
            throw new BadRequestException($"No such data exist. ");
        }

        public async Task Cancel(string orderId, string refusedReason, string userId)
        {
            var order = await _readOnlyRepository.GetFirstAsync<DealingOrder>(o => 
                o.Id == orderId && o.UserId == userId);
            if (order == null)
            {
                throw new BadRequestException($"No such order in our db. ");
            }
            var dishes = JsonConvert.DeserializeObject<List<OrderDishDto>>(order.Dishes);

            var archive = await _repository.CreateAsync(new ArchivedOrder
            {
                Id = orderId,
                RestaurantId = order.RestaurantId,
                SeatId = order.SeatId,
                CenterId = order.CenterId,
                UserId = order.UserId,
                FetchNumber = order.FetchNumber,
                OrderNumber = order.OrderNumber,
                RefusedReason = refusedReason,
                ContactPhone = order.ContactPhone,
                Note = order.Note,
                IsDishPacked = order.IsDishPacked,
                PaymentType = order.PaymentType,
                DeliveryType = order.DeliveryType,
                IsMerchantCanceled = true,
                Status = OrderStatus.Closed,
                CreatedTime = order.CreatedTime,
                Bill = new Bill
                {
                    Id = Guid.NewGuid().ToUuidString(),
                    IsPaid = false,
                    Amount = GetTotal(dishes)
                }
            });

            if (dishes != null && dishes.Any())
            {
                var orderDishes = new List<Order_Dish>();
                var orderDish_Customizations = new List<OrderDish_Customization>();

                dishes.ForEach(dish => {
                    var orderDishId = Guid.NewGuid().ToUuidString();
                    orderDishes.Add(new Order_Dish
                    {
                        Id = orderDishId,
                        DishId = dish.Id,
                        DishUnitPrice = dish.UnitPrice,
                        DishName = dish.Name,
                        OrderId = archive.Id,
                        Amount = dish.Amount,
                        RestaurantId = order.RestaurantId
                    });

                    if (dish.Customizations != null && dish.Customizations.Any())
                    {
                        dish.Customizations.ForEach(c => {
                            orderDish_Customizations.Add(
                                new OrderDish_Customization
                                {
                                    Id = Guid.NewGuid().ToUuidString(),
                                    OrderDishId = orderDishId,
                                    OrderId = archive.Id,
                                    CustomizationName = c.Name,
                                    CustomizationUnitPrice = c.UnitPrice,
                                    RestaurantId = order.RestaurantId
                                });
                        });
                    }
                });

                await _repository.CreateRangeAsync(orderDishes);
                await _repository.CreateRangeAsync(orderDish_Customizations);
            }
            else
            {
                throw new Exception($"Dishes information is missing in that order, order id {order.Id}");
            }
            _repository.Delete(order);

            var detail = await _readOnlyRepository.GetFirstAsync<OrderDetail>(od =>
                od.OrderId == orderId);

            if (detail == null)
            {
                var orderDetail = new OrderDetail
                {
                    Id = Guid.NewGuid().ToUuidString(),
                    OrderId = orderId
                };

                orderDetail.AnyToClosed = DateTime.UtcNow;

                await _repository.CreateAsync(orderDetail);
            }
            else
            {
                detail.AnyToClosed = DateTime.UtcNow;
                _repository.Update(detail);
            }
        }

        private async Task<PagedList<OrderRoughResult>> GetAllArchive(string userId, int pageIndex, int pageSize)
        {
            var totalCount = await _readOnlyRepository.GetCountAsync<ArchivedOrder>(order =>
                    order.UserId == userId);

            if (totalCount == 0)
            {
                return new PagedList<OrderRoughResult>();
            }

            var result = new PagedList<OrderRoughResult>
            {
                Count = totalCount
            };

            var orders = (await _readOnlyRepository.GetAllAsync<ArchivedOrder>(order =>
                order.UserId == userId, order =>
                order.OrderByDescending(o => o.CreatedTime), "Restaurant,Bill,Seat",
                pageIndex * pageSize, pageSize)).ToList();

            //all dishes that user has been ordered. 
            var dishes = (await _readOnlyRepository.GetAllAsync<Order_Dish>(order_Dish =>
                order_Dish.Order.UserId == userId, null, "Order")).ToList();

            //all customization that user has been ordered. 
            var customizations = (await _readOnlyRepository.GetAllAsync<OrderDish_Customization>(odc =>
                dishes.Select(d => d.Id).Contains(odc.OrderDishId))).ToList();


            var orderList = new List<OrderRoughResult>();

            orders.ForEach(order => {
                var roughOrder = new OrderRoughResult
                {
                    Id = order.Id,
                    RestaurantName = order.Restaurant.Name,
                    SeatNo = order.Seat?.Name,
                    Status = order.Status,
                    IsDishPacked = order.IsDishPacked,
                    DeliveryType = order.DeliveryType,
                    FetchNumber = order.FetchNumber,
                    CreatedTime = order.CreatedTime,
                    Dishes = dishes.Where(d => d.OrderId == order.Id).Select(d => d.DishName).ToList()
                };
                roughOrder.TotalBill = GetTotal(order.Id, dishes, customizations);
                orderList.Add(roughOrder);
            });
            result.Entities = orderList;

            return result;
        }

        private decimal GetTotal(string orderId, List<Order_Dish> dishes, List<OrderDish_Customization> customizations)
        {
            var total = default(decimal);
            var orderDishes = dishes.Where(d => d.OrderId == orderId);
            foreach (var orderDish in orderDishes)
            {
                var customizationsForDish = customizations.Where(c => c.OrderDishId == orderDish.Id);

                var suitePrice = (customizationsForDish.Sum(cfd => cfd.CustomizationUnitPrice) + orderDish.DishUnitPrice) * orderDish.Amount;
                total += suitePrice;
            }

            return total;
        }


        private async Task<OrderDetailResult> GetDealingOrderDetail(string orderId)
        {
            var order = await _readOnlyRepository.GetFirstAsync<DealingOrder>(o => o.Id == orderId, null, "Restaurant,Seat");

            if (order == null) return null;

            var detail = await _readOnlyRepository.GetFirstAsync<OrderDetail>(o =>
                o.OrderId == orderId);

            var orderResponseTime = order.Restaurant.OrderResponseTime ?? 15;
            int? countdown = null;

            if ((DateTime.UtcNow - order.CreatedTime).TotalMinutes < orderResponseTime)
            {
                countdown = (int)(orderResponseTime * 60 - (DateTime.UtcNow - order.CreatedTime).TotalSeconds);
            }

            var orderDetail = new OrderDetailResult
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                CreatedAt = order.CreatedTime,
                StatusTime = GetOrderStatusTime(detail, order.Status),
                CountDown = countdown,
                RestaurantName = order.Restaurant.Name,
                SeatName = order.Seat?.Name,
                IsPacked = order.IsDishPacked ? 1 : 0,
                Note = order.Note,
                Status = order.Status,
                DeliveryType = order.DeliveryType,
                PaymentType = order.PaymentType,
                FetchNumber = order.FetchNumber,
                Phone = order.ContactPhone,
                Dishes = JsonConvert.DeserializeObject<List<OrderDishDto>>(order.Dishes)
            };

            var totalBill = default(decimal);
            var amountOfDishes = default(byte);
            foreach (var dish in orderDetail.Dishes)
            {
                dish.SuitePrice = GetSuitePrice(dish);
                if (dish.Customizations != null && dish.Customizations.Any())
                {
                    dish.CustomizationContent = string.Join(',', dish.Customizations.Select(c => c.Name));
                }
                totalBill += dish.SuitePrice;
                amountOfDishes += dish.Amount;
            }

            orderDetail.TotalBill = totalBill;
            orderDetail.AmountOfDishes = amountOfDishes;

            return orderDetail;
        }

        private async Task<OrderDetailResult> GetArchiveOrderDetail(string orderId)
        {
            var result = new OrderDetailResult();

            var order = await _readOnlyRepository.GetFirstAsync<ArchivedOrder>(o =>
                o.Id == orderId, null, "Restaurant,Seat");

            var detail = await _readOnlyRepository.GetFirstAsync<OrderDetail>(o =>
                o.OrderId == orderId);

            if (order == null)
            {
                throw new BadRequestException($"No such order found in db, order id {orderId}");
            }

            var orderResponseTime = order.Restaurant.OrderResponseTime ?? 15;

            var dishes = (await _readOnlyRepository.GetAllAsync<Order_Dish>(order_Dish =>
                order_Dish.OrderId == orderId)).ToList();

            var customizations = (await _readOnlyRepository.GetAllAsync<OrderDish_Customization>(order_custom =>
                order_custom.OrderId == orderId)).ToList();

            var orderDishes = GetDishsAndRelatedCustomizations(orderId, dishes, customizations);

            result.Id = order.Id;
            result.OrderNumber = order.OrderNumber;
            result.Note = order.Note;
            result.CreatedAt = order.CreatedTime;
            result.StatusTime = GetOrderStatusTime(detail, order.Status);
            result.CountDown = null;
            result.RestaurantName = order.Restaurant.Name;
            result.SeatName = order.Seat?.Name;
            result.Status = order.Status;
            result.DeliveryType = order.DeliveryType;
            result.PaymentType = order.PaymentType;
            result.FetchNumber = order.FetchNumber;
            result.Phone = order.ContactPhone;
            result.RefusedReason = order.RefusedReason;
            result.Dishes = orderDishes;
            result.AmountOfDishes = (byte)(orderDishes.Sum(od => od.Amount));
            result.TotalBill = GetTotal(orderDishes);

            return result;
        }

        private string MaskOrderIndex(long orderIndex)
        {
            // sample: 00015
            var indexStr = orderIndex.ToString();
            return indexStr.PadLeft(5, '0');
        }

        private async Task<long> UpdateAndFetchDailyOrderIndex()
        {
            var redisDb = _connectionMultiplexer.GetDatabase();
            return await redisDb.StringIncrementAsync(AppConsts.RedisKey_OrderDailyIndex, 1);
        }

        private async Task<long> UpdateAndFetchNumberForRestaurant(string restaurantId)
        {
            var redisDb = _connectionMultiplexer.GetDatabase();
            return await redisDb.StringIncrementAsync($"{AppConsts.RedisKey_RestaurantOrderFetchNumberPrefix}{restaurantId}", 1);
        }               

        private decimal GetTotal(List<OrderDishDto> orderDishes)
        {
            var total = default(decimal);
            orderDishes.ForEach(dish => {
                total += GetSuitePrice(dish);
            });
            return total;
        }

        private decimal GetSuitePrice(OrderDishDto dish)
        {
            var suitPrice = dish.UnitPrice;
            if (dish.Customizations != null && dish.Customizations.Any())
            {
                foreach (var customization in dish.Customizations)
                {
                    suitPrice += customization.UnitPrice;
                }
            }
            return suitPrice * dish.Amount;
        }

        private DateTime? GetOrderStatusTime(OrderDetail detail, OrderStatus status)
        {
            if (detail == null || status == OrderStatus.Pending)
            {
                return null;
            }

            if (status == OrderStatus.Cooking)
            {
                return detail.PendingToCooking;
            }
            if (status == OrderStatus.DeliveringOrTaking)
            {
                return detail.CookingToDeliveOrTaking;
            }
            if (status == OrderStatus.Closed)
            {
                return detail.AnyToClosed;
            }
            if (status == OrderStatus.Done)
            {
                return detail.DeliveOrTakingToDone;
            }
            return null;
        }

        private List<OrderDishDto> GetDishsAndRelatedCustomizations(string orderId, List<Order_Dish> dishes,
            List<OrderDish_Customization> customizations)
        {
            dishes = dishes.Where(o =>
                o.OrderId == orderId).ToList();

            customizations = customizations.Where(odc =>
                odc.OrderId == orderId).ToList();

            var orderDishes = new List<OrderDishDto>();

            if (customizations == null)
            {
                //there is no any customizations at this order
                orderDishes = dishes.Select(dish => new OrderDishDto
                {
                    Id = dish.Id,
                    Name = dish.DishName,
                    UnitPrice = dish.DishUnitPrice,
                    Amount = dish.Amount,
                    SuitePrice = dish.DishUnitPrice * dish.Amount,
                    CustomizationContent = null,
                    Customizations = null
                }).ToList();
            }
            else
            {
                orderDishes = (from dish in dishes
                               join customization in customizations on dish.Id equals customization.OrderDishId
                               into dish_customizations
                               from dish_customization in dish_customizations.DefaultIfEmpty()
                               group new { dish, dish_customization } by dish
                            into temp
                               select new OrderDishDto
                               {
                                   Id = temp.Key.DishId,
                                   Name = temp.Key.DishName,
                                   UnitPrice = temp.Key.DishUnitPrice,
                                   Amount = temp.Key.Amount,
                                   Customizations = temp.Where(t => t.dish_customization != null).Select(t =>
                                   new OrderDishDto.CustomizationDto
                                   {
                                       Name = t.dish_customization?.CustomizationName,
                                       UnitPrice = t.dish_customization?.CustomizationUnitPrice ?? default(decimal)
                                   }).ToList()
                               }).ToList();

                orderDishes.ForEach(dish => {
                    if (dish.Customizations != null && dish.Customizations.Any())
                    {
                        dish.CustomizationContent = string.Join('/', dish.Customizations.Select(c => c.Name));
                        dish.SuitePrice = GetSuitePrice(dish);
                    }
                });
            }

            return orderDishes;
        }
    }
}
