using Newtonsoft.Json;
using SFood.DataAccess.Common.Enums;
using SFood.DataAccess.EFCore;
using SFood.DataAccess.Infrastructure.Interfaces;
using SFood.DataAccess.Models;
using SFood.DataAccess.Models.RelationshipModels;
using SFood.MerchantEndpoint.Application.Dtos.Internal;
using SFood.MerchantEndpoint.Application.Dtos.Parameters.Order;
using SFood.MerchantEndpoint.Application.Dtos.Results.Order;
using SFood.MerchantEndpoint.Common.Exceptions;
using SFood.MerchantEndpoint.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Application.ServiceImplements
{
    /// <summary>
    /// 订单服务
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly SFoodDbContext _context;
        private readonly IRepository _repository;
        private readonly IReadOnlyRepository _readOnlyRepository;

        public OrderService(IRepository repository,
            IReadOnlyRepository readOnlyRepository,
            SFoodDbContext context)
        {
            _repository = repository;
            _readOnlyRepository = readOnlyRepository;
            _context = context;
        }

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="orderId"></param>
        public async Task<OrderDetailResult> GetDetail(string orderId, string restaurantId)
        {
            var dealingOrder = await RetrieveDealingOrderDetailInfo(orderId);
            if (dealingOrder != null)
            {
                return dealingOrder;
            }
            var archivedOrder = await RetrieveArchivedOrderDetailInfo(orderId, restaurantId);
            if (archivedOrder != null)
            {
                return archivedOrder;
            }
            throw new BadRequestException($"No such order in our db, order id: {orderId}");
        }

        /// <summary>
        /// 获取今日订单量
        /// </summary>
        /// <param name="restaurantId">餐厅id</param>
        /// <returns>订单的数量</returns>
        public async Task<int> GetTodayCount(string restaurantId)
        {
            var restaurant = await _readOnlyRepository.GetFirstAsync<Restaurant>(r => 
                r.Id == restaurantId && r.Status == RestaurantStatus.Running);
            if (restaurant == null)
            {
                throw new Exception($"No such restaurant exist, restaurantId : {restaurantId}");
            }

            var count = await _readOnlyRepository.GetCountAsync<ArchivedOrder>(order =>
                order.RestaurantId == restaurantId &&
                order.Status == OrderStatus.Done &&
                order.CreatedTime.Date == DateTime.UtcNow.Date);

            var countOfDealing = await _readOnlyRepository.GetCountAsync<DealingOrder>(order =>
                order.RestaurantId == restaurantId &&
                order.CreatedTime.Date == DateTime.UtcNow.Date);
            return count + countOfDealing;
        }

        public async Task ChangeOrderStatus(ChangeOrderStatusParam param)
        {
            var order = await _readOnlyRepository.GetFirstAsync<DealingOrder>(o => o.Id == param.OrderId);
            if (order == null)
            {
                throw new BadRequestException($"No such order in our db. ");
            }
            else
            {
                if (order.Status != param.FromStatus)
                {
                    throw new BadRequestException($"That order is on {order.Status.ToString()} status. ");
                }

                if (order.Status == OrderStatus.Pending)
                {
                    //Checking if the order has been expired.
                    var restaurant = await _readOnlyRepository.GetFirstAsync<Restaurant>(rd =>
                        rd.Id == param.RestaurantId, null, "RestaurantDetail");
                    if (!restaurant.RestaurantDetail.IsReceivingAuto)
                    {
                        var responseTime = restaurant.OrderResponseTime ?? 15;
                        if ((DateTime.UtcNow - order.CreatedTime).TotalMinutes >= responseTime)
                        {                           
                            throw new BadRequestException($"The order has been expired based on your pre-configed order response time. ");
                        }
                    }                    
                }

                if (param.ToStatus == OrderStatus.Done || param.ToStatus == OrderStatus.Closed)
                {
                    await TransferDealingToArchive(order.Id, param.ToStatus, param.RefusedReason);
                }
                else
                {
                    order.Status = param.ToStatus;
                    _repository.Update(order);
                }                
            }
            await UpdateOrderDetail(order.Id, param.FromStatus, param.ToStatus);
        }        

        public async Task<TodayOrderListResult> GetTodayList(OrderStatus status, bool isPaged, string restaurantId, int pageIndex, int pageSize)
        {
            if (status.IsDealingOrder())
            {
                return await GetTodayDealingList(status, isPaged, restaurantId, pageIndex, pageSize);
            }
            else
            {
                return await GetTodayArchivedList(status, isPaged, restaurantId, pageIndex, pageSize);
            }
        }

        public async Task<AllOrderListResult> GetAllList(int status, string restaurantId, int pageIndex, int pageSize)
        {
            if (status != -1)
            {
                var orderStatus = (OrderStatus)status;
                if (orderStatus.IsDealingOrder())
                {
                    return await GetAllDealingList(orderStatus, restaurantId, pageIndex, pageSize);
                }
                else
                {
                    return await GetAllArchivedList(orderStatus, restaurantId, pageIndex, pageSize);
                }
            }
            else
            {
                var result = new AllOrderListResult();
                var pagedEntities = await GetAllOrderList(restaurantId, pageIndex, pageSize);
                result.TotalCount = pagedEntities.Count;
                result.Orders = pagedEntities.Entities;
                result.IsLastPage = IsLastPage(result.TotalCount, pageSize, pageIndex);

                return result;
            }            
        }



        private async Task<TodayOrderListResult> GetTodayDealingList(OrderStatus status, bool isPaged, string restaurantId, int? pageIndex, int? pageSize)
        {
            var totalCount = await _readOnlyRepository.GetCountAsync<DealingOrder>(order =>
                    order.RestaurantId == restaurantId &&
                    order.Status == status &&
                    order.CreatedTime.Date == DateTime.UtcNow.Date);

            if (totalCount == 0)
            {
                return new TodayOrderListResult();
            }

            if (!isPaged)
            {
                pageIndex = null;
                pageSize = null;
            }

            var result = new TodayOrderListResult {
                TotalCount = totalCount,
                IsLastPage = isPaged ? IsLastPage(totalCount, pageSize.Value, pageIndex.Value) : true
            };            

            var orders = (await _readOnlyRepository.GetAllAsync<DealingOrder>(order =>
                    order.RestaurantId == restaurantId &&
                    order.Status == status && 
                    order.CreatedTime.Date == DateTime.UtcNow.Date, order =>
                    order.OrderBy(o => o.CreatedTime), "Seat",
                    pageIndex * pageSize, pageSize)).ToList();
            
            var orderList = new List<OrderRoughResult>();
            orders.ForEach(order => {
                var roughOrder = new OrderRoughResult
                {
                    Id = order.Id,
                    Note = order.Note,
                    SeatName = order.Seat?.Name,
                    Status = order.Status,
                    IsDishPacked = order.IsDishPacked ? 1 : 0,
                    DeliveryType = order.DeliveryType,
                    FetchNumber = order.FetchNumber,
                    CreatedTime = order.CreatedTime,
                    CreatedTimeDescription = order.CreatedTime.TimeDescription(),
                    IsLastPage = result.IsLastPage,
                    Dishes = JsonConvert.DeserializeObject<List<OrderDishResult>>(order.Dishes)
                };
                roughOrder.TotalBill = GetTotal(roughOrder.Dishes);
                roughOrder.Dishes.ForEach(dish => {                   
                    dish.SuitePrice = GetSuitePrice(dish);
                    if (dish.Customizations != null && dish.Customizations.Any())
                    {
                        dish.CustomizationContent = string.Join('/', dish.Customizations.Select(c => c.Name));
                    }
                });
                orderList.Add(roughOrder);
            });

            result.Orders = orderList;
            return result;
        }

        private async Task<TodayOrderListResult> GetTodayArchivedList(OrderStatus status, bool isPaged, string restaurantId, int? pageIndex, int? pageSize)
        {
            var totalCount = await _readOnlyRepository.GetCountAsync<ArchivedOrder>(order =>
                    order.RestaurantId == restaurantId &&
                    order.Status == status &&
                    order.CreatedTime.Date == DateTime.UtcNow.Date);

            if (totalCount == 0)
            {
                return new TodayOrderListResult();
            }

            if (!isPaged)
            {
                pageIndex = null;
                pageSize = null;
            }

            var result = new TodayOrderListResult
            {
                TotalCount = totalCount,
                IsLastPage = isPaged ? IsLastPage(totalCount, pageSize.Value, pageIndex.Value) : true
            };

            var orders = (await _readOnlyRepository.GetAllAsync<ArchivedOrder>(order =>
                order.RestaurantId == restaurantId && 
                order.Status == status && 
                order.CreatedTime.Date == DateTime.UtcNow.Date, order =>
                order.OrderBy(o => o.CreatedTime), "Seat",
                pageIndex * pageSize, pageSize)).ToList();

            var dishes = (await _readOnlyRepository.GetAllAsync<Order_Dish>(order_Dish =>
                order_Dish.RestaurantId == restaurantId)).ToList();

            var customizations = (await _readOnlyRepository.GetAllAsync<OrderDish_Customization>(order_custom =>
                order_custom.RestaurantId == restaurantId)).ToList();


            var orderList = new List<OrderRoughResult>();

            orders.ForEach(order => {
                var roughOrder = new OrderRoughResult
                {
                    Id = order.Id,
                    Note = order.Note,
                    SeatName = order.Seat?.Name,
                    Status = order.Status,
                    IsDishPacked = order.IsDishPacked ? 1 : 0,
                    DeliveryType = order.DeliveryType,
                    FetchNumber = order.FetchNumber,
                    CreatedTime = order.CreatedTime,
                    CreatedTimeDescription = order.CreatedTime.TimeDescription(),
                    IsLastPage = result.IsLastPage,
                    Dishes = GetDishsAndRelatedCustomizations(order.Id, dishes, customizations)
                };
                roughOrder.TotalBill = GetTotal(roughOrder.Dishes);
                orderList.Add(roughOrder);
            });
            result.Orders = orderList;

            return result;
        }



        private async Task<AllOrderListResult> GetAllDealingList(OrderStatus status, string restaurantId, int pageIndex, int pageSize)
        {
            var totalCount = await _readOnlyRepository.GetCountAsync<DealingOrder>(order =>
                    order.RestaurantId == restaurantId &&
                    order.Status == status);

            if (totalCount == 0)
            {
                return new AllOrderListResult();
            }

            var result = new AllOrderListResult {
                TotalCount = totalCount,
                IsLastPage = IsLastPage(totalCount, pageSize, pageIndex)
            };

            var orders = (await _readOnlyRepository.GetAllAsync<DealingOrder>(order =>
                    order.RestaurantId == restaurantId &&
                    order.Status == status, order =>
                    order.OrderByDescending(o => o.CreatedTime), "Seat",
                    pageIndex * pageSize, pageSize)).ToList();

            var orderList = new List<OrderRoughResult>();
            orders.ForEach(order => {
                var roughOrder = new OrderRoughResult
                {
                    Id = order.Id,
                    Note = order.Note,
                    SeatName = order.Seat?.Name,
                    Status = order.Status,
                    IsDishPacked = order.IsDishPacked ? 1 : 0,
                    DeliveryType = order.DeliveryType,
                    FetchNumber = order.FetchNumber,
                    CreatedTime = order.CreatedTime,
                    CreatedTimeDescription = order.CreatedTime.TimeDescription(),
                    IsLastPage = result.IsLastPage,
                    Dishes = JsonConvert.DeserializeObject<List<OrderDishResult>>(order.Dishes)
                };
                roughOrder.TotalBill = GetTotal(roughOrder.Dishes);
                roughOrder.Dishes.ForEach(dish => {                    
                    dish.SuitePrice = GetSuitePrice(dish);
                    if (dish.Customizations != null && dish.Customizations.Any())
                    {
                        dish.CustomizationContent = string.Join('/', dish.Customizations.Select(c => c.Name));
                    }
                });
                orderList.Add(roughOrder);
            });

            result.Orders = orderList;
            return result;
        }

        private async Task<AllOrderListResult> GetAllArchivedList(OrderStatus status, string restaurantId, int pageIndex, int pageSize)
        {
            var totalCount = await _readOnlyRepository.GetCountAsync<ArchivedOrder>(order =>
                    order.RestaurantId == restaurantId &&
                    order.Status == status);

            if (totalCount == 0)
            {
                return new AllOrderListResult();
            }

            var result = new AllOrderListResult {
                TotalCount = totalCount,
                IsLastPage = IsLastPage(totalCount, pageSize, pageIndex)
            };

            var orders = (await _readOnlyRepository.GetAllAsync<ArchivedOrder>(order =>
                order.RestaurantId == restaurantId &&
                order.Status == status, order =>
                order.OrderByDescending(o => o.CreatedTime), "Seat",
                pageIndex * pageSize, pageSize)).ToList();

            var dishes = (await _readOnlyRepository.GetAllAsync<Order_Dish>(order_Dish =>
                order_Dish.RestaurantId == restaurantId)).ToList();

            var customizations = (await _readOnlyRepository.GetAllAsync<OrderDish_Customization>(order_custom =>
                order_custom.RestaurantId == restaurantId)).ToList();
            
            var orderList = new List<OrderRoughResult>();

            orders.ForEach(order => {
                var roughOrder = new OrderRoughResult
                {
                    Id = order.Id,
                    Note = order.Note,
                    SeatName = order.Seat?.Name,
                    Status = order.Status,
                    IsDishPacked = order.IsDishPacked ? 1 : 0,
                    DeliveryType = order.DeliveryType,
                    FetchNumber = order.FetchNumber,
                    CreatedTime = order.CreatedTime,
                    CreatedTimeDescription = order.CreatedTime.TimeDescription(),
                    IsLastPage = result.IsLastPage,
                    Dishes = GetDishsAndRelatedCustomizations(order.Id, dishes, customizations)
                };
                roughOrder.TotalBill = GetTotal(roughOrder.Dishes);
                roughOrder.Dishes.ForEach(dish => {                    
                    dish.SuitePrice = GetSuitePrice(dish);
                    if (dish.Customizations != null && dish.Customizations.Any())
                    {
                        dish.CustomizationContent = string.Join('/', dish.Customizations.Select(c => c.Name));
                    }
                });
                orderList.Add(roughOrder);
            });
            result.Orders = orderList;

            return result;
        }


        private async Task<PagedList<OrderRoughResult>> GetAllOrderList(string restaurantId, int pageIndex, int pageSize)
        {            
            var dealings = await GetAllDealingList(restaurantId, pageIndex, pageSize);
            var result = new PagedList<OrderRoughResult>();

            if (((pageIndex + 1) * pageSize) <= dealings.Count)
            {
                var archivedPart = await GetAllArchivedList(restaurantId, 0, pageSize);
                //situation 1 : all entities are from dealing orders
                result.Count = dealings.Count + archivedPart.Count;
                result.Entities = dealings.Entities;
                return result;
            }
            else if (((pageIndex + 1) * pageSize) - dealings.Count <= pageSize)
            {
                //situation 2 : part entities are from dealing orders and part are from archived orders
                var dealingPart = await GetAllDealingList(restaurantId, pageIndex, pageSize);
                var archivedPart = await GetAllArchivedList(restaurantId, 0, pageSize - dealingPart.Count % pageSize);

                result.Count = dealings.Count + archivedPart.Count;
                result.Entities.AddRange(dealingPart.Entities);
                result.Entities.AddRange(archivedPart.Entities);

                return result;
            }
            else
            {
                //situation 3 : all entities are from archived orders
                var newPageIndex = (((pageIndex + 1) * pageSize) - dealings.Count) / pageSize - 1;
                var archivedPart = await GetAllArchivedList(restaurantId, newPageIndex, pageSize);

                result.Count = dealings.Count + archivedPart.Count;
                result.Entities.AddRange(archivedPart.Entities);
                return result;
            }
        }

        /// <summary>
        /// 获取活动订单的分页数据，
        /// 默认排序为时间倒序
        /// </summary>
        /// <param name="restaurantId">餐厅id</param>
        /// <param name="pageIndex">分页index</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        private async Task<PagedList<OrderRoughResult>> GetAllDealingList(string restaurantId, int pageIndex, int pageSize)
        {
            var totalCount = await _readOnlyRepository.GetCountAsync<DealingOrder>(order =>
                    order.RestaurantId == restaurantId);

            if (totalCount == 0)
            {
                return new PagedList<OrderRoughResult>();
            }

            var result = new PagedList<OrderRoughResult>
            {
                Count = totalCount
            };
            var orders = (await _readOnlyRepository.GetAllAsync<DealingOrder>(order =>
                    order.RestaurantId == restaurantId, order =>
                    order.OrderByDescending(o => o.CreatedTime), "Seat",
                    pageIndex * pageSize, pageSize)).ToList();

            var orderList = new List<OrderRoughResult>();
            orders.ForEach(order => {
                var roughOrder = new OrderRoughResult
                {
                    Id = order.Id,
                    Note = order.Note,
                    SeatName = order.Seat?.Name,
                    Status = order.Status,
                    IsDishPacked = order.IsDishPacked ? 1 : 0,
                    DeliveryType = order.DeliveryType,
                    FetchNumber = order.FetchNumber,
                    CreatedTime = order.CreatedTime,
                    CreatedTimeDescription = order.CreatedTime.TimeDescription(),
                    Dishes = JsonConvert.DeserializeObject<List<OrderDishResult>>(order.Dishes)
                };
                roughOrder.TotalBill = GetTotal(roughOrder.Dishes);
                roughOrder.Dishes.ForEach(dish => {
                    dish.SuitePrice = GetSuitePrice(dish);
                    if (dish.Customizations != null && dish.Customizations.Any())
                    {
                        dish.CustomizationContent = string.Join('/', dish.Customizations.Select(c => c.Name));
                    }
                });
                orderList.Add(roughOrder);
            });

            result.Entities = orderList;
            return result;
        }

        /// <summary>
        /// 获取历史订单的分页数据，
        /// 默认排序为时间倒序
        /// </summary>
        /// <param name="restaurantId">餐厅id</param>
        /// <param name="pageIndex">分页index</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        private async Task<PagedList<OrderRoughResult>> GetAllArchivedList(string restaurantId, int pageIndex, int pageSize)
        {
            var totalCount = await _readOnlyRepository.GetCountAsync<ArchivedOrder>(order =>
                    order.RestaurantId == restaurantId);

            if (totalCount == 0)
            {
                return new PagedList<OrderRoughResult>();
            }

            var result = new PagedList<OrderRoughResult>
            {
                Count = totalCount
            };

            var orders = (await _readOnlyRepository.GetAllAsync<ArchivedOrder>(order =>
                order.RestaurantId == restaurantId, order =>
                order.OrderByDescending(o => o.CreatedTime), "Seat",
                pageIndex * pageSize, pageSize)).ToList();

            var dishes = (await _readOnlyRepository.GetAllAsync<Order_Dish>(order_Dish =>
                order_Dish.RestaurantId == restaurantId)).ToList();

            var customizations = (await _readOnlyRepository.GetAllAsync<OrderDish_Customization>(order_custom =>
                order_custom.RestaurantId == restaurantId)).ToList();

            var orderList = new List<OrderRoughResult>();

            orders.ForEach(order => {
                var roughOrder = new OrderRoughResult
                {
                    Id = order.Id,
                    Note = order.Note,
                    SeatName = order.Seat?.Name,
                    Status = order.Status,
                    IsDishPacked = order.IsDishPacked ? 1 : 0,
                    DeliveryType = order.DeliveryType,
                    FetchNumber = order.FetchNumber,
                    CreatedTime = order.CreatedTime,
                    CreatedTimeDescription = order.CreatedTime.TimeDescription(),
                    Dishes = GetDishsAndRelatedCustomizations(order.Id, dishes, customizations)
                };
                roughOrder.TotalBill = GetTotal(roughOrder.Dishes);
                roughOrder.Dishes.ForEach(dish => {
                    dish.SuitePrice = GetSuitePrice(dish);
                    if (dish.Customizations != null && dish.Customizations.Any())
                    {
                        dish.CustomizationContent = string.Join('/', dish.Customizations.Select(c => c.Name));
                    }
                });
                orderList.Add(roughOrder);
            });
            result.Entities = orderList;

            return result;
        }



        private async Task<OrderDetailResult> RetrieveDealingOrderDetailInfo(string orderId)
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

            var orderDetail = new OrderDetailResult {
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
                Dishes = JsonConvert.DeserializeObject<List<OrderDishResult>>(order.Dishes)
            };

            var totalBill = default(decimal);
            var amountOfDishes = default(byte);
            foreach (var dish in orderDetail.Dishes)
            {
                dish.SuitePrice = GetSuitePrice(dish);
                if (dish.Customizations != null && dish.Customizations.Any())
                {
                    dish.CustomizationContent = string.Join('/', dish.Customizations.Select(c => c.Name));
                }
                totalBill += dish.SuitePrice;
                amountOfDishes += dish.Amount;
            }

            orderDetail.TotalBill = totalBill;
            orderDetail.AmountOfDishes = amountOfDishes;

            return orderDetail;
        }

        private async Task<OrderDetailResult> RetrieveArchivedOrderDetailInfo(string orderId, string restaurantId)
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
                order_Dish.RestaurantId == restaurantId)).ToList();

            var customizations = (await _readOnlyRepository.GetAllAsync<OrderDish_Customization>(order_custom =>
                order_custom.RestaurantId == restaurantId)).ToList();

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


        private List<OrderDishResult> GetDishsAndRelatedCustomizations(string orderId, List<Order_Dish> dishes,
            List<OrderDish_Customization> customizations)
        {
            dishes = dishes.Where(o =>
                o.OrderId == orderId).ToList();

            customizations = customizations.Where(odc =>
                odc.OrderId == orderId).ToList();

            var orderDishes = new List<OrderDishResult>();

            if (customizations == null)
            {
                //there is no any customizations at this order
                orderDishes = dishes.Select(dish => new OrderDishResult
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
                               select new OrderDishResult
                               {
                                   Id = temp.Key.DishId,
                                   Name = temp.Key.DishName,
                                   UnitPrice = temp.Key.DishUnitPrice,
                                   Amount = temp.Key.Amount,
                                   Customizations = temp.Where(t => t.dish_customization != null).Select(t =>
                                   new OrderCustomizationDto
                                   {
                                       Name = t.dish_customization?.CustomizationName,
                                       UnitPrice = t.dish_customization?.CustomizationUnitPrice?? default(decimal)
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

        private async Task UpdateOrderDetail(string orderId, OrderStatus from, OrderStatus to)
        {
            var detail = await _readOnlyRepository.GetFirstAsync<OrderDetail>(od =>
                od.OrderId == orderId);
            if (detail == null)
            {
                var orderDetail = new OrderDetail
                {
                    Id = Guid.NewGuid().ToUuidString(),
                    OrderId = orderId
                };

                if (from == OrderStatus.Pending && to == OrderStatus.Cooking)
                {
                    orderDetail.PendingToCooking = DateTime.UtcNow;
                }

                if (from == OrderStatus.Cooking && to == OrderStatus.DeliveringOrTaking)
                {
                    orderDetail.CookingToDeliveOrTaking = DateTime.UtcNow;
                }

                if (from == OrderStatus.DeliveringOrTaking && to == OrderStatus.Done)
                {
                    orderDetail.DeliveOrTakingToDone = DateTime.UtcNow;
                }

                if (to == OrderStatus.Closed)
                {
                    orderDetail.AnyToClosed = DateTime.UtcNow;
                }

                await _repository.CreateAsync(orderDetail);
            }
            else
            {
                if (from == OrderStatus.Pending && to == OrderStatus.Cooking)
                {
                    detail.PendingToCooking = DateTime.UtcNow;
                }

                if (from == OrderStatus.Cooking && to == OrderStatus.DeliveringOrTaking)
                {
                    detail.CookingToDeliveOrTaking = DateTime.UtcNow;
                }

                if (from == OrderStatus.DeliveringOrTaking && to == OrderStatus.Done)
                {
                    detail.DeliveOrTakingToDone = DateTime.UtcNow;
                }

                if (to == OrderStatus.Closed)
                {
                    detail.AnyToClosed = DateTime.UtcNow;
                }
                _repository.Update(detail);
            }
        }

        private async Task TransferDealingToArchive(string orderId, OrderStatus toStatus, string refusedReason)
        {
            var order = await _readOnlyRepository.GetFirstAsync<DealingOrder>(o => o.Id == orderId);
            var dishes = JsonConvert.DeserializeObject<List<OrderDishResult>>(order.Dishes);
            
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
                Status = toStatus,
                CreatedTime = order.CreatedTime,
                Bill = new Bill
                {
                    Id = Guid.NewGuid().ToUuidString(),
                    IsPaid = toStatus == OrderStatus.Done,
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
        }

        private decimal GetTotal(List<OrderDishResult> orderDishes)
        {
            var total = default(decimal);
            orderDishes.ForEach(dish => {
                total += GetSuitePrice(dish);
            });
            return total;
        }

        private bool IsLastPage(int total, int pageSize, int pageIndex)
        {
            return Math.Ceiling((double)total / pageSize) <= pageIndex + 1;
        }

        private decimal GetSuitePrice(OrderDishResult dish)
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
    }
}
