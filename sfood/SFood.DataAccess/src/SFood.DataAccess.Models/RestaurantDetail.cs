using SFood.DataAccess.Common.Consts;
using SFood.DataAccess.Common.Enums;
using SFood.DataAccess.Models.Infrastructure;
using SFood.DataAccess.Models.Infrastructure.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFood.DataAccess.Models
{
    [Table("Details", Schema = "Restaurant")]
    public class RestaurantDetail : UuidEnity, IHasCreatedTime, IHasModifiedTime
    {
        /// <summary>
        /// 餐厅id
        /// </summary>
        [MaxLength(DbConst.KeyLength)]
        public string RestaurantId { get; set; }

        /// <summary>
        /// 国家区域代码
        /// </summary>
        [MaxLength(DbConst.KeyLength)]
        public string CountryCodeId { get; set; }

        /// <summary>
        /// 餐厅注册的进展
        /// </summary>
        [MaxLength(DbConst.Length_30)]
        public RestaurantRegistrationStatus? RegistrationStatus { get; set; }

        /// <summary>
        /// 审核通过的时间
        /// </summary>
        public DateTime? QualifiedTime { get; set; }

        /// <summary>
        /// 申请类型
        /// </summary>
        public MerchantApplicationType ApplicationType { get; set; }

        /// <summary>
        /// 餐厅在center的号码
        /// </summary>
        [MaxLength(DbConst.Length_20)]
        public string RestaurantNo { get; set; }

        /// <summary>
        /// 开店时间
        /// </summary>
        public short? OpenedAt { get; set; }

        /// <summary>
        ///  闭店时间
        /// </summary>
        public short? ClosedAt { get; set; }

        /// <summary>
        /// 自动接单
        /// </summary>
        public bool IsReceivingAuto { get; set; }

        /// <summary>
        /// 餐厅的电话
        /// </summary>
        /// <value></value>
        [MaxLength(DbConst.PhoneLength)]
        public string Phone { get; set; }

        /// <summary>
        /// 餐厅的完整介绍
        /// </summary>
        /// <value></value>
        [MaxLength(DbConst.Length_2000)]
        public string Introduction { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime? LastModifiedTime { get; set; }

        [ForeignKey(nameof(RestaurantId))]
        public virtual Restaurant Restaurant { get; set; }        
    }
}