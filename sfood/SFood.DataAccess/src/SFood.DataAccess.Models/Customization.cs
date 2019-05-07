using SFood.DataAccess.Common.Consts;
using SFood.DataAccess.Models.Infrastructure;
using SFood.DataAccess.Models.Infrastructure.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFood.DataAccess.Models
{
    [Table("Customizations", Schema = "Dish")]
    public class Customization: UuidEnity, IHasCreatedTime, IHasModifiedTime, ISoftDelete
    {
        /// <summary>
        /// 菜品定制项名字
        /// </summary>
        [MaxLength(DbConst.Length_100)]
        public string Name { get; set; }

        /// <summary>
        /// 多规格的选项值顺序
        /// </summary>
        public byte Index { get; set; }

        /// <summary>
        /// 菜品定制项单价
        /// </summary>
        [Column(TypeName = DbConst.MoneyDecimal)]
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// 是否是默认项
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// 定制项分类id
        /// </summary>
        [MaxLength(DbConst.KeyLength)]
        public string CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public virtual CustomizationCategory Category { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime? LastModifiedTime { get; set; }

        public bool IsDeleted { get; set; }
    }
}
