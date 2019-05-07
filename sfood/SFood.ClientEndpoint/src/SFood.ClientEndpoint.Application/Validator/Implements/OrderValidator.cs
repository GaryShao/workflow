using Microsoft.EntityFrameworkCore;
using SFood.ClientEndpoint.Application.Dtos.Internal;
using SFood.ClientEndpoint.Common.Exceptions;
using SFood.DataAccess.EFCore;
using SFood.DataAccess.Infrastructure.Interfaces;
using SFood.DataAccess.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SFood.ClientEndpoint.Application.Validator.Implements
{
    public class OrderValidator : IOrderValidator
    {
        private readonly IReadOnlyRepository _readonlyRepository;
        private readonly SFoodDbContext _dbContext;

        public OrderValidator(IReadOnlyRepository readonlyRepository
            , SFoodDbContext dbContext)
        {
            _readonlyRepository = readonlyRepository;
            _dbContext = dbContext;
        }

        /// <summary>
        /// Validate the customizations of each dishes from 3 aspects:
        /// <para>1. if the dish price right </para> 
        /// <para>2. if the customization id belongs to the dish id</para> 
        /// <para>3. if the customization price right</para> 
        /// </summary>
        /// <param name="orderDishes"></param>
        /// <returns></returns>
        public async Task ValidateOrderInfo(List<OrderDishDto> orderDishes, string restaurantId)
        {
            var dishes = (await _readonlyRepository.GetAllAsync<Dish>(d => 
                d.RestaurantId == restaurantId)).ToList();            

            var dishIds = orderDishes.Select(dish => dish.Id).ToList();

            //var param = new SqlParameter("@DishIds", string.Join(',', dishIds));

            //var dishCustomizations = _dbContext.USP_DishCustomizations.
            //    FromSql("Dish.uspGetDishCustomizations @DishIds", param).ToList();

            //orderDishes.ForEach(dish => {

            //    var isDishPriceRight = dish.UnitPrice == dishes.First(d => d.Id == dish.Id).UnitPrice;

            //    if (!isDishPriceRight)
            //    {
            //        throw new BadRequestException($"Your order has price change, please refresh your app and order again. ");
            //    }

            //    if (dish.Customizations != null && dish.Customizations.Any())
            //    {
            //        dish.Customizations.ForEach(customization => {
            //            var isPriceRight = customization.UnitPrice == dishCustomizations.First(c => 
            //                c.CustomizationId == customization.Id).CustomizationUnitPrice;
            //            if (!isPriceRight)
            //            {
            //                throw new BadRequestException($"Your order has price change, please refresh your app and order again. ");
            //            }
            //        });
            //    }
            //});
        }
    }
}
