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
    /// 食阁座位区域列表
    /// </summary>
    [Table("SeatAreas", Schema = "HawkerCenter")]
    public class SeatArea: UuidEnity, ISoftDelete,IHasCreatedTime
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
        /// 名称
        /// </summary>
        [MaxLength(DbConst.Length_20)]
        public string Name { get; set; }
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

        /// <summary>
        /// 桌位信息
        /// </summary>
        public virtual ICollection<Seat> Seats { get; set; }
    }
}
