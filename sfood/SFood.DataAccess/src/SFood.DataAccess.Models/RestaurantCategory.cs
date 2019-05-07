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
    [Table("Categories", Schema = "Restaurant")]
    public class RestaurantCategory : UuidEnity, IHasCreatedTime, IHasModifiedTime
    {
        /// <summary>
        /// 分类的名字
        /// </summary>
        /// <value></value>
        [MaxLength(DbConst.Length_100)]
        public string Name { get; set; }

        /// <summary>
        /// 正常的图标
        /// </summary>
        [MaxLength(DbConst.ImageUrlLength)]
        public string Icon { get; set; }

        /// <summary>
        /// 被选中时的图标
        /// </summary>        
        [MaxLength(DbConst.ImageUrlLength)]
        public string SelectedIcon { get; set; }

        public DateTime? LastModifiedTime { get; set;  }

        public DateTime CreatedTime { get; set;  }

        public virtual IList<Restaurant_RestaurantCategory> Restaurant_RestaurantCategories { get; set; }
    }
}