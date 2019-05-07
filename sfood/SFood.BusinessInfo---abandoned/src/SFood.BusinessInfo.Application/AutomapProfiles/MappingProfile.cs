using AutoMapper;
using SFood.BusinessInfo.Application.Dtos.Responses;
using SFood.DataAccess.Models;
using SFood.DataAccess.Models.RelationshipModels;

namespace SFood.BusinessInfo.Application.AutomapProfiles
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            Bootstrap();
        }

        private void Bootstrap()
        {
            //CreateMap<source, destination>();

            CreateMap<RestaurantCategory, RestaurantCategoryDto>();
            CreateMap<DishCategory, DishCategoryRoughDto>();
            CreateMap<Dish, DishDto>();
            CreateMap<Order_Dish, OrderDishDto>().
                ForMember(dto => dto.Id, opt => opt.MapFrom(src => src.DishId)).
                ForMember(dto => dto.Name, opt => opt.MapFrom(src => src.DishName)).
                ForMember(dto => dto.UnitPrice, opt => opt.MapFrom(src => src.DishUnitPrice)).
                ForMember(dto => dto.Amount, opt => opt.MapFrom(src => src.Amount));

            CreateMap<DealingOrder, RoughOrderDto>();
            CreateMap<DealingOrder, DetailedOrderDto>();
            CreateMap<ArchivedOrder, DetailedOrderDto>();
        }
    }
}
