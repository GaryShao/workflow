using SFood.DataAccess.Common.Consts;
using SFood.DataAccess.Common.Enums;
using SFood.DataAccess.Models.Infrastructure;
using SFood.DataAccess.Models.Infrastructure.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFood.DataAccess.Models
{
    [Table("Dealings", Schema = "OrderInfo")]
    public class DealingOrder: UuidEnity, IHasCreatedTime, IHasModifiedTime
    {
        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderStatus Status { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        [MaxLength(DbConst.Length_20)]
        public string OrderNumber { get; set; }

        /// <summary>
        /// 配送方式
        /// </summary>
        public DeliveryType DeliveryType { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public PaymentType PaymentType { get; set; }

        /// <summary>
        /// 食物是否打包
        /// </summary>
        public bool IsDishPacked { get; set; }

        /// <summary>
        /// 订单备注
        /// </summary>
        [MaxLength(DbConst.Length_500)]
        public string Note { get; set; }

        /// <summary>
        /// 具体的食物列表json
        /// </summary>
        [MaxLength(DbConst.Length_2000)]
        public string Dishes { get; set; }

        /// <summary>
        /// 座位号
        /// </summary>
        [MaxLength(DbConst.KeyLength)]
        public string SeatId { get; set; }

        /// <summary>
        /// 食阁id
        /// </summary>
        [MaxLength(DbConst.KeyLength)]
        public string CenterId { get; set; }

        /// <summary>
        /// 餐厅id
        /// </summary>
        [MaxLength(DbConst.KeyLength)]
        public string RestaurantId { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [MaxLength(DbConst.PhoneLength)]
        public string ContactPhone { get; set; }

        /// <summary>
        /// 下单用户id
        /// </summary>
        [MaxLength(DbConst.KeyLength)]
        public string UserId { get; set; }

        /// <summary>
        /// 取餐码
        /// </summary>
        [MaxLength(DbConst.Length_10)]
        public string FetchNumber { get; set; }

        [ForeignKey(nameof(SeatId))]
        public virtual Seat Seat { get; set; }

        [ForeignKey(nameof(CenterId))]
        public virtual HawkerCenter Center { get; set; }

        [ForeignKey(nameof(RestaurantId))]
        public virtual Restaurant Restaurant { get; set; }

        public DateTime? LastModifiedTime { get; set; }

        public DateTime CreatedTime { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }
    }
}
