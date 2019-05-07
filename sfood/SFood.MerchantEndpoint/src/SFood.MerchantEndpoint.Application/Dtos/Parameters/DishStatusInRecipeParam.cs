namespace SFood.MerchantEndpoint.Application.Dtos.Parameters
{
    public class DishStatusInRecipeParam
    {
        public string RecipeId { get; set; }

        public string DishId { get; set; }

        public bool SetAsOnShelf { get; set; }
    }
}
