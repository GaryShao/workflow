using SFood.DataAccess.Common.Consts;
using SFood.DataAccess.Common.Enums;
using SFood.DataAccess.Models.Infrastructure;
using SFood.DataAccess.Models.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFood.DataAccess.Models
{
    [Table("BasicInfos", Schema = "HawkerCenter")]
    public class HawkerCenter : UuidEnity, ISoftDelete, IHasCreatedTime, IHasModifiedTime
    {        
        /// <summary>
        /// hawker center 的名字
        /// </summary>
        /// <value></value>
        [MaxLength(DbConst.Length_100)]
        public string Name { get; set; }
        /// <summary>
        /// hawker center 的详细地址
        /// </summary>
        /// <value></value>
        [MaxLength(DbConst.Length_500)]
        public string DetailAddress { get; set; }
        /// <summary>
        /// hawker center 所在的区
        /// </summary>
        /// <value></value>
        [MaxLength(DbConst.Length_100)]
        public string District { get; set; }

        /// <summary>
        /// 食阁状态
        /// </summary>
        [MaxLength(DbConst.Length_30)]
        public HawkerCenterStatus Status { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? LastModifiedTime { get; set; }

        public DateTime CreatedTime { get; set; }

        public virtual HawkerCenterDetail HawkerCenterDetail { get; set; }

        public virtual ICollection<Restaurant> Restaurants { get; set; }        
    }
}
