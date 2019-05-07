using SFood.DataAccess.Common.Consts;
using SFood.DataAccess.Models.Infrastructure;
using SFood.DataAccess.Models.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SFood.DataAccess.Models
{
    /// <summary>
    /// 食阁Banner
    /// </summary>
    [Table("Banners",Schema = "HawkerCenter")]
    public class HawkerCenterBanner : UuidEnity, IHasCreatedTime
    {
        /// <summary>
        /// 食阁ID
        /// </summary>
        [MaxLength(DbConst.KeyLength)]
        public string CenterId { get; set; }

        /// <summary>
        /// 食阁信息
        /// </summary>
        [ForeignKey(nameof(CenterId))]
        public virtual HawkerCenter Center { get; set; }

        /// <summary>
        /// 跳转链接
        /// </summary>
        [MaxLength(DbConst.Length_1000)]
        public string TargetUrl { get; set; }

        /// <summary>
        /// 生效开始时间
        /// </summary>
        public DateTime StartAt { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndAt { get; set; }

        /// <summary>
        /// 访问次数
        /// </summary>
        public int Visit { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [MaxLength(DbConst.Length_100)]
        public string CreatedUserName { get; set; }

    }
}
