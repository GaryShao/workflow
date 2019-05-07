using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFood.BackendService.Application.Dtos;
using SFood.BackendService.Application.ServiceInterfaces;
using SFood.BackendService.Common.Extensions;
using SFood.DataAccess.Common.Enums;
using SFood.DataAccess.Infrastructure.Interfaces;
using SFood.DataAccess.Models;
using SFood.DataAccess.Models.RelationshipModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFood.BackendService.Application.ServiceImplements
{
    public class OrderService : IOrderService
    {
        private readonly ILogger _logger;
        private readonly IRepository _repository;
        private readonly IReadOnlyRepository _readOnlyRepository;

        public OrderService(ILoggerFactory loggerFactory
            , IRepository repository
            , IReadOnlyRepository readOnlyRepository)
        {
            _logger = loggerFactory.CreateLogger(nameof(OrderService));
            _repository = repository;
            _readOnlyRepository = readOnlyRepository;
        }

        /// <summary>
        /// 正常流程： 把刚过期的订单标记为关闭状态，
        /// 并且移到archive表中
        /// </summary>
        /// <returns></returns>
        public async Task CancelOrdersAsync()
        {
            var restaurants = (await _readOnlyRepository.GetAllAsync<Restaurant>(r => r.IsOpened && !r.RestaurantDetail.IsReceivingAuto, null, "RestaurantDetail")).Select(r =>
                 new RestaurantDto
                 {
                     Id = r.Id,
                     OrderResponseTime = r.OrderResponseTime ?? 15
                 }).ToList();

            var orders = (await _readOnlyRepository.GetAllAsync<DealingOrder>(order =>
                order.Status == OrderStatus.Pending)).ToList();

            restaurants.ForEach(async restaurant => {
                var responseTime = restaurant.OrderResponseTime;

                var ordersGonnaExpired = orders.Where(order => order.RestaurantId == restaurant.Id &&
                    (DateTime.UtcNow - order.CreatedTime).TotalSeconds >= (responseTime) * 60).ToList();

                //1. remove expired order from dealingorder table
                _repository.DeleteRange(ordersGonnaExpired);

                if (ordersGonnaExpired != null && ordersGonnaExpired.Any())
                {
                    var orderDetails = new List<OrderDetail>();
                    var archivedOrders = new List<ArchivedOrder>();
                    var orderDishes = new List<Order_Dish>();
                    var orderDish_Customizations = new List<OrderDish_Customization>();


                    foreach (var order in ordersGonnaExpired)
                    {
                        order.Status = OrderStatus.Closed;
                        var dishes = JsonConvert.DeserializeObject<List<OrderDishResult>>(order.Dishes);
                        var archive = new ArchivedOrder
                        {
                            Id = order.Id,
                            RestaurantId = order.RestaurantId,
                            OrderNumber = order.OrderNumber,
                            SeatId = order.SeatId,
                            CenterId = order.CenterId,
                            UserId = order.UserId,
                            FetchNumber = order.FetchNumber,                            
                            ContactPhone = order.ContactPhone,
                            RefusedReason = "超时未接单",
                            Note = order.Note,
                            IsDishPacked = order.IsDishPacked,
                            PaymentType = order.PaymentType,
                            DeliveryType = order.DeliveryType,
                            IsMerchantCanceled = true,
                            Status = order.Status,
                            CreatedTime = order.CreatedTime
                        };

                        archivedOrders.Add(archive);
                        orderDetails.Add(new OrderDetail
                        {
                            Id = Guid.NewGuid().ToUuidString(),
                            OrderId = order.Id,
                            AnyToClosed = DateTime.UtcNow
                        });

                        if (dishes != null && dishes.Any())
                        {                            
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
                        }
                        else
                        {
                            throw new Exception($"Dishes information is missing in that order, order id {order.Id}");
                        }
                    }

                    // create detail for orders in batch
                    await _repository.CreateRangeAsync(archivedOrders);
                    await _repository.CreateRangeAsync(orderDetails);
                    await _repository.CreateRangeAsync(orderDishes);
                    await _repository.CreateRangeAsync(orderDish_Customizations);

                    _repository.Save();
                }
            });
        }

        /// <summary>
        /// 防止有遗漏的数据
        /// </summary>
        /// <returns></returns>
        public async Task TransferClosedOrders()
        {
            var closedOrders = (await _readOnlyRepository.GetAllAsync<DealingOrder>(order =>
                order.Status == OrderStatus.Closed)).ToList();

            if (closedOrders != null && closedOrders.Any())
            {
                var newOrderDetails = new List<OrderDetail>();
                var orderDetailsToUpdate = new List<OrderDetail>();
                var archivedOrders = new List<ArchivedOrder>();
                var orderDishes = new List<Order_Dish>();
                var orderDish_Customizations = new List<OrderDish_Customization>();


                foreach (var order in closedOrders)
                {
                    var dishes = JsonConvert.DeserializeObject<List<OrderDishResult>>(order.Dishes);
                    var archive = new ArchivedOrder
                    {
                        Id = order.Id,
                        RestaurantId = order.RestaurantId,
                        OrderNumber = order.OrderNumber,
                        SeatId = order.SeatId,
                        CenterId = order.CenterId,
                        UserId = order.UserId,
                        FetchNumber = order.FetchNumber,
                        ContactPhone = order.ContactPhone,
                        RefusedReason = "超时未接单",
                        Note = order.Note,
                        IsDishPacked = order.IsDishPacked,
                        PaymentType = order.PaymentType,
                        DeliveryType = order.DeliveryType,
                        IsMerchantCanceled = true,
                        Status = order.Status,
                        CreatedTime = order.CreatedTime
                    };

                    archivedOrders.Add(archive);
                    var detail = await _readOnlyRepository.GetFirstAsync<OrderDetail>(od => od.OrderId == order.Id);

                    if (detail == null)
                    {
                        newOrderDetails.Add(new OrderDetail
                        {
                            Id = Guid.NewGuid().ToUuidString(),
                            OrderId = order.Id,
                            AnyToClosed = DateTime.UtcNow
                        });
                    }
                    else
                    {
                        detail.AnyToClosed = DateTime.UtcNow;
                        orderDetailsToUpdate.Add(detail);
                    }                                                                                            

                    if (dishes != null && dishes.Any())
                    {
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
                    }
                    else
                    {
                        throw new Exception($"Dishes information is missing in that order, order id {order.Id}");
                    }
                }

                // create detail for orders in batch
                await _repository.CreateRangeAsync(archivedOrders);
                await _repository.CreateRangeAsync(newOrderDetails);
                await _repository.CreateRangeAsync(orderDishes);
                await _repository.CreateRangeAsync(orderDish_Customizations);
                _repository.UpdateRange(orderDetailsToUpdate);

                _repository.Save();
            }

        }
    }
}
