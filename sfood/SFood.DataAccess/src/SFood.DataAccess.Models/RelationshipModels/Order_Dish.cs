using SFood.DataAccess.Common.Consts;
using SFood.DataAccess.Models.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFood.DataAccess.Models.RelationshipModels
{
    [Table("Orders&Dishes", Schema = "RelationShip")]
    public class Order_Dish : UuidEnity
    {
        [MaxLength(DbConst.KeyLength)]
        public string DishId { get; set; }

        [Column(TypeName = DbConst.MoneyDecimal)]
        public decimal DishUnitPrice { get; set; }

        [MaxLength(DbConst.Length_100)]
        public string DishName { get; set; }

        [MaxLength(DbConst.KeyLength)]
        public string OrderId { get; set; }

        [MaxLength(DbConst.KeyLength)]
        public string RestaurantId { get; set; }

        public byte Amount { get; set; }

        public virtual ArchivedOrder Order { get; set; }
        public virtual Dish Dish { get; set; }        

        public virtual IList<OrderDish_Customization> OrderDish_Customizations { get; set; }
    }
}
