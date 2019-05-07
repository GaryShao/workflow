using AutoMapper;

namespace SFood.ClientEndpoint.Application.Configurations
{
    public class ApplicationProfile: Profile
    {
        public ApplicationProfile()
        {
            Bootstrap();
        }

        private void Bootstrap()
        {
            //CreateMap<CreateMenuParam, Menu>();
            //CreateMap<EditRecipeParam, Menu>();
            //CreateMap<Menu, MenuBasicResult>();

            //CreateMap<CreateDishCategoryParam, DishCategory>();
        }
    }
}
