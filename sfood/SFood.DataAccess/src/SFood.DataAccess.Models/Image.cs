using SFood.DataAccess.Common.Consts;
using SFood.DataAccess.Common.Enums;
using SFood.DataAccess.Models.Infrastructure;
using SFood.DataAccess.Models.Infrastructure.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFood.DataAccess.Models
{
    [Table("Images", Schema = "Common")]
    public class Image: UuidEnity, IHasCreatedTime, IHasModifiedTime
    {
        /// <summary>
        /// 图片url
        /// </summary>
        [MaxLength(DbConst.ImageUrlLength)]
        public string Url { get; set; }

        /// <summary>
        /// 餐厅的图片分类
        /// </summary>
        public RestaurantImageCategory? RestaurantImageCategory { get; set; }

        public DateTime? LastModifiedTime { get; set; }

        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 餐厅id
        /// </summary>
        [MaxLength(DbConst.KeyLength)]
        public string RestaurantId { get; set; }

        [ForeignKey(nameof(RestaurantId))]
        public virtual Restaurant Restaurant { get; set; }        
    }
}