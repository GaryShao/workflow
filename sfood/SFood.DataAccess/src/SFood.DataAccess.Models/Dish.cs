using SFood.DataAccess.Common.Consts;
using SFood.DataAccess.Models.Infrastructure;
using SFood.DataAccess.Models.Infrastructure.Interfaces;
using SFood.DataAccess.Models.RelationshipModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFood.DataAccess.Models
{
    [Table("Dishes", Schema = "Restaurant")]
    public class Dish: UuidEnity, IHasCreatedTime, IHasModifiedTime, ISoftDelete
    {     
        /// <summary>
        /// 菜品名称
        /// </summary>
        [MaxLength(DbConst.Length_100)]
        public string Name  { get; set; }
        /// <summary>
        /// 菜品单价
        /// </summary>
        [Column(TypeName = DbConst.MoneyDecimal)]
        public decimal UnitPrice { get; set; }
        /// <summary>
        /// 月销量
        /// </summary>
        public int SalesVolumeMonth { get; set; }
        /// <summary>
        /// 年销量
        /// </summary>
        public int SalesVolumeAnnual { get; set; }
        /// <summary>
        /// 人工设置销量
        /// </summary>
        public int SalesVolumeManual { get; set; }
        /// <summary>
        /// 菜品图标 
        /// </summary>
        [MaxLength(DbConst.ImageUrlLength)]
        public string Icon { get; set; }

        [MaxLength(DbConst.KeyLength)]
        public string RestaurantId { get; set; }

        [MaxLength(DbConst.KeyLength)]
        public string CenterId { get; set; }

        [ForeignKey(nameof(RestaurantId))]
        public virtual Restaurant Restaurant { get; set; }

        [ForeignKey(nameof(CenterId))]
        public virtual HawkerCenter Center { get; set; }

        public virtual IList<Dish_DishCategory> Dish_DishCategories { get; set; }

        public virtual IList<Order_Dish> Order_Dishes { get; set; }

        public virtual IList<Dish_CustomizationCategory> Dish_CustomizationCategories { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime? LastModifiedTime { get; set; }

        public bool IsDeleted { get; set; }
    }
}