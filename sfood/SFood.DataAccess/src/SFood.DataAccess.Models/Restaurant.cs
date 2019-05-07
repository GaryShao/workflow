using SFood.DataAccess.Common.Consts;
using SFood.DataAccess.Common.Enums;
using SFood.DataAccess.Models.Infrastructure;
using SFood.DataAccess.Models.Infrastructure.Interfaces;
using SFood.DataAccess.Models.RelationshipModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFood.DataAccess.Models
{
    [Table("BasicInfos", Schema = "Restaurant")]
    public class Restaurant : UuidEnity, IHasCreatedTime, IHasModifiedTime
    {        
        /// <summary>
        /// 餐厅名字
        /// </summary>
        /// <value></value>
        [MaxLength(DbConst.Length_100)]
        public string Name { get; set; }

        /// <summary>
        /// 餐厅logo
        /// </summary>
        /// <value></value>
        [MaxLength(DbConst.ImageUrlLength)]
        public string Logo { get; set; }

        /// <summary>
        /// 餐厅的简介
        /// </summary>
        /// <value></value>
        [MaxLength(DbConst.Length_500)]
        public string Announcement { get; set; }

        /// <summary>
        /// 餐厅是否营业
        /// </summary>
        /// <value></value>        
        public bool IsOpened { get; set; }

        /// <summary>
        /// 餐厅是否支持送餐
        /// </summary>
        /// <value></value>
        public bool IsDeliverySupport { get; set; }

        /// <summary>
        /// 店铺设置的未处理订单的失效时间
        /// </summary>
        public byte? OrderResponseTime { get; set; }

        /// <summary>
        /// 餐厅默认排序的权重因子
        /// </summary>
        /// <value></value>
        public int SortWeight { get; set; }

        /// <summary>
        /// 月销量
        /// </summary>
        /// <value></value>
        public int SalesVolumeMonth { get; set; }

        /// <summary>
        /// 年销量
        /// </summary>
        /// <value></value>
        public int SalesVolumeAnnual { get; set; }

        /// <summary>
        /// 手动设置销量
        /// </summary>
        /// <value></value>
        public int SalesVolumeManual { get; set; }

        /// <summary>
        /// 餐厅的状态
        /// </summary>
        public RestaurantStatus Status { get; set; }

        [MaxLength(DbConst.KeyLength)]
        public string CenterId { get; set; }

        [ForeignKey(nameof(CenterId))]
        public virtual HawkerCenter Center { get; set; }

        public virtual RestaurantDetail RestaurantDetail { get; set; }

        public virtual IList<Restaurant_RestaurantCategory> Restaurant_RestaurantCategories { get; set; }

        public DateTime? LastModifiedTime { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}