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
    [Table("DishCategories", Schema = "Restaurant")]
    public class DishCategory : UuidEnity, IHasCreatedTime, IHasModifiedTime
    {
        /// <summary>
        /// 分类名字(菜单下的菜品分类)
        /// </summary>
        [MaxLength(DbConst.Length_100)]
        public string Name { get; set; }

        /// <summary>
        /// 分类的顺序
        /// </summary>
        public byte Index { get; set; }

        /// <summary>
        /// 所属餐厅id
        /// </summary>
        [MaxLength(DbConst.KeyLength)]
        public string RestaurantId { get; set; }

        /// <summary>
        /// 所属菜单id
        /// </summary>
        [MaxLength(DbConst.KeyLength)]
        public string MenuId { get; set; }

        [ForeignKey(nameof(RestaurantId))]
        public virtual Restaurant Restaurant { get; set; }

        [ForeignKey(nameof(MenuId))]
        public virtual Menu Menu { get; set; }

        public virtual IList<Dish_DishCategory> Dish_DishCategories { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime? LastModifiedTime { get; set; }
    }
}
