using AutoMapper;
using SFood.DataAccess.Models;
using SFood.MerchantEndpoint.Application.Dtos.Parameters;
using SFood.MerchantEndpoint.Application.Dtos.Parameters.Menu;
using SFood.MerchantEndpoint.Application.Dtos.Results.Menu;

namespace SFood.MerchantEndpoint.Application.Configurations
{
    public class ApplicationProfile: Profile
    {
        public ApplicationProfile()
        {
            Bootstrap();
        }

        private void Bootstrap()
        {
            CreateMap<CreateMenuParam, Menu>();
            CreateMap<EditRecipeParam, Menu>();
            CreateMap<Menu, MenuBasicResult>();

            CreateMap<CreateDishCategoryParam, DishCategory>();
        }
    }
}
