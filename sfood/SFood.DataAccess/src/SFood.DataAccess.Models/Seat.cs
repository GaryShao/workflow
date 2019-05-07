using SFood.DataAccess.Common.Consts;
using SFood.DataAccess.Models.Infrastructure;
using SFood.DataAccess.Models.Infrastructure.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFood.DataAccess.Models
{
    [Table("Seats", Schema = "HawkerCenter")]
    public class Seat: UuidEnity, ISoftDelete, IHasCreatedTime
    {
        /// <summary>
        /// 桌位的名字
        /// </summary>
        [MaxLength(DbConst.Length_20)]
        public string Name { get; set; }


        /// <summary>
        /// 食阁ID
        /// </summary>
        [MaxLength(DbConst.KeyLength)]
        public string CenterId { get; set; }

        /// <summary>
        /// 座位区域
        /// </summary>
        [MaxLength(DbConst.KeyLength)]
        public  string SeatAreaId { get; set; }
        
        /// <summary>
        /// 桌位所属食阁
        /// </summary>
        [ForeignKey(nameof(CenterId))]
        public virtual HawkerCenter Center { get; set; }

        /// <summary>
        /// 食阁区域
        /// </summary>
        [ForeignKey(nameof(SeatAreaId))]
        public virtual SeatArea Area { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }

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