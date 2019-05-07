using SFood.DataAccess.Common.Consts;
using SFood.DataAccess.Models.Infrastructure;
using SFood.DataAccess.Models.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFood.DataAccess.Models
{
    [Table("CustomizationCategories", Schema = "Dish")]
    public class CustomizationCategory : UuidEnity, IHasCreatedTime, IHasModifiedTime, ISoftDelete
    {
        /// <summary>
        /// 定制项分类的名字
        /// </summary>
        [MaxLength(DbConst.Length_100)]
        public string Name { get; set; }

        /// <summary>
        /// 是否是多选
        /// </summary>
        public bool IsMultiple { get; set; }

        /// <summary>
        /// 是否勾选
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// 定制项类别的最大可选数目
        /// </summary>
        public byte MaxOptions { get; set; }

        /// <summary>
        /// 这个规格继承于哪个预设规格
        /// </summary>
        [MaxLength(DbConst.KeyLength)]
        public string FromId { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime? LastModifiedTime { get; set; }

        /// <summary>
        /// 判断是否是系统预设值
        /// </summary>
        public bool IsSystem { get; set; }

        public byte Index { get; set; }

        [MaxLength(DbConst.KeyLength)]
        public string RestaurantId { get; set; }

        [MaxLength(DbConst.KeyLength)]
        public string RestaurantCategoryId { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<Customization> Customizations { get; set; }
    }
}
