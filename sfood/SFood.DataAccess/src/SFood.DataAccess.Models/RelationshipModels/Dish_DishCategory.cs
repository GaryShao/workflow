using SFood.DataAccess.Common.Consts;
using SFood.DataAccess.Models.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFood.DataAccess.Models.RelationshipModels
{
    [Table("Dishes&Categories", Schema = "RelationShip")]
    public class Dish_DishCategory : UuidEnity
    {        
        public byte Index { get; set; }

        public bool IsOnShelf { get; set; }

        [MaxLength(DbConst.KeyLength)]
        public string DishId { get; set; }

        [MaxLength(DbConst.KeyLength)]
        public string DishCategoryId { get; set; }

        public virtual Dish Dish { get; set; }

        public virtual DishCategory Category { get; set; }

        [MaxLength(DbConst.KeyLength)]
        public string MenuId { get; set; }

        [MaxLength(DbConst.KeyLength)]
        public string RestaurantId { get; set; }
    }
}
