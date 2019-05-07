using SFood.DataAccess.Common.Consts;
using SFood.DataAccess.Models.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFood.DataAccess.Models.RelationshipModels
{
    [Table("Restaurant&Categories", Schema = "RelationShip")]
    public class Restaurant_RestaurantCategory : UuidEnity
    {
        /// <summary>
        /// 餐厅id
        /// </summary>
        [MaxLength(DbConst.KeyLength)]
        public string RestaurantId { get; set; }

        /// <summary>
        /// 餐厅类别id
        /// </summary>
        [MaxLength(DbConst.KeyLength)]
        public string RestaurantCategoryId { get; set; }

        public virtual Restaurant Restaurant { get; set; }

        public virtual RestaurantCategory RestaurantCategory { get; set; }
    }
}
