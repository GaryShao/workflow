using SFood.DataAccess.Common.Consts;
using SFood.DataAccess.Models.Infrastructure;
using SFood.DataAccess.Models.Infrastructure.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFood.DataAccess.Models
{
    [Table("Details", Schema = "HawkerCenter")]
    public class HawkerCenterDetail : UuidEnity, IHasCreatedTime, IHasModifiedTime 
    {
        /// <summary>
        /// 食阁id
        /// </summary>
        [MaxLength(DbConst.KeyLength)]
        public string CenterId { get; set; }

        /// <summary>
        /// ???
        /// </summary>
        public int Capacity { get; set; }

        /// <summary>
        /// ???
        /// </summary>
        public long FundFlow { get; set; } 

        /// <summary>
        /// 餐厅数量
        /// </summary>
        public int AmountOfRestaurants { get; set; }

        /// <summary>
        /// 联系人姓名
        /// </summary>
        [MaxLength(DbConst.Length_100)]
        public string ContactName { get; set; }

        /// <summary>
        /// 联系人手机
        /// </summary>
        [MaxLength(DbConst.Length_20)]
        public string ContactPhone { get; set; }

        public DateTime? LastModifiedTime { get; set; }

        public DateTime CreatedTime { get; set; }

        [ForeignKey(nameof(CenterId))]
        public virtual HawkerCenter Center { get; set; }
    }
}