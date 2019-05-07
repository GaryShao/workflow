using SFood.DataAccess.Common.Consts;
using SFood.DataAccess.Common.Enums;
using SFood.DataAccess.Models.Infrastructure;
using SFood.DataAccess.Models.Infrastructure.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFood.DataAccess.Models
{
    [Table("Qualifications", Schema = "Restaurant")]
    public class Qualification : UuidEnity, IHasCreatedTime, IHasModifiedTime
    {
        /// <summary>
        /// 餐厅id
        /// </summary>
        [MaxLength(DbConst.KeyLength)]
        public string RestaurantId { get; set; }

        /// <summary>
        /// 审核条目
        /// </summary>
        public MerchantQualificationEntry Entry { get; set; } 
        
        /// <summary>
        /// 条目内容
        /// </summary>
        [MaxLength(DbConst.Length_500)]
        public string Value { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public bool? IsQualified { get; set; }

        /// <summary>
        /// 未过审原因
        /// </summary>
        [MaxLength(DbConst.Length_500)]
        public string Reason { get; set; }

        [MaxLength(DbConst.KeyLength)]
        public string EditorId { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime? LastModifiedTime { get; set; }        
    }
}
