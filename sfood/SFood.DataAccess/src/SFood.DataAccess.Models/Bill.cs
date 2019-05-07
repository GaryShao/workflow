using SFood.DataAccess.Common.Consts;
using SFood.DataAccess.Models.Infrastructure;
using SFood.DataAccess.Models.Infrastructure.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFood.DataAccess.Models
{
    [Table("Bills", Schema = "OrderInfo")]
    public class Bill: UuidEnity, IHasCreatedTime
    {
        /// <summary>
        /// 是否已支付
        /// </summary>
        public bool IsPaid { get; set; }

        /// <summary>
        /// 订单的总金额
        /// </summary>
        [Column(TypeName = DbConst.MoneyDecimal)]
        public decimal Amount { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        [MaxLength(DbConst.KeyLength)]
        public string OrderId { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}
