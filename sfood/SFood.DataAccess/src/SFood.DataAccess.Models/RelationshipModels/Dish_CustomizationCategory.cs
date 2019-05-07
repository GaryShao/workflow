using SFood.DataAccess.Models.Infrastructure;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFood.DataAccess.Models.RelationshipModels
{
    [Table("Dishes&CustomizationCategories", Schema = "RelationShip")]
    public class Dish_CustomizationCategory : UuidEnity
    {
        /// <summary>
        /// 菜品id
        /// </summary>
        public string DishId { get; set; }

        /// <summary>
        /// 规格id
        /// </summary>
        public string CustomizationCategoryId { get; set; }
    }
}
