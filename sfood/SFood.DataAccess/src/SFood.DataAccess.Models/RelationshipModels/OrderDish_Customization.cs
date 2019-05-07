using SFood.DataAccess.Common.Consts;
using SFood.DataAccess.Models.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFood.DataAccess.Models.RelationshipModels
{
    [Table("OrderDishes&Customizations", Schema = "RelationShip")]
    public class OrderDish_Customization : UuidEnity
    {
        [MaxLength(DbConst.KeyLength)]
        public string OrderId { get; set; }

        public string RestaurantId { get; set; }

        [Column(TypeName = DbConst.MoneyDecimal)]
        public decimal CustomizationUnitPrice { get; set; }

        [MaxLength(DbConst.Length_20)]
        public string CustomizationName { get; set; }

        [MaxLength(DbConst.KeyLength)]
        public string OrderDishId { get; set; }

        public virtual Order_Dish OrderDish { get; set; }
    }
}
