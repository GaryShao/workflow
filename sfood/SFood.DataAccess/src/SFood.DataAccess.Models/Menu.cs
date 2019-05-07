using SFood.DataAccess.Common.Consts;
using SFood.DataAccess.Models.Infrastructure;
using SFood.DataAccess.Models.Infrastructure.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFood.DataAccess.Models
{
    [Table("Menus", Schema = "Restaurant")]
    public class Menu : UuidEnity, IHasCreatedTime, IHasModifiedTime
    {
        /// <summary>
        /// 菜单名字
        /// </summary>
        [MaxLength(DbConst.Length_100)]
        public string Name { get; set; }

        /// <summary>
        /// 菜单生效时间
        /// </summary>
        public short BeginTime { get; set; }

        /// <summary>
        /// 菜单失效时间
        /// </summary>
        public short EndTime { get; set; }

        /// <summary>
        /// 是否是系统预设的默认菜单
        /// </summary>
        public bool IsDefault { get; set; }

        [MaxLength(DbConst.KeyLength)]        
        public string RestaurantId { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime? LastModifiedTime { get; set; }

        [ForeignKey(nameof(RestaurantId))]
        public virtual Restaurant Restaurant { get; set; }
    }
}
