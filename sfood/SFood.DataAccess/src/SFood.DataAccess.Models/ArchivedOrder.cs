using SFood.DataAccess.Common.Consts;
using SFood.DataAccess.Common.Enums;
using SFood.DataAccess.Models.Infrastructure;
using SFood.DataAccess.Models.Infrastructure.Interfaces;
using SFood.DataAccess.Models.RelationshipModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFood.DataAccess.Models
{
    [Table("Archives", Schema = "OrderInfo")]
    public class ArchivedOrder: UuidEnity, IHasCreatedTime, IHasModifiedTime
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
        /// 取消订单分为2种：客户取消 | 商户取消
        /// </summary>
        public bool IsMerchantCanceled { get; set; }

        /// <summary>
        /// 配送类型
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
        /// 座位id
        /// </summary>
        [MaxLength(DbConst.KeyLength)]
        public string SeatId { get; set; }

        /// <summary>
        /// 食阁id
        /// </summary>
        [MaxLength(DbConst.KeyLength)]
        public string CenterId { get; set; }

        /// <summary>
        /// 订单账单记录
        /// </summary>
        [MaxLength(DbConst.KeyLength)]
        public string BillId { get; set; }

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
        /// 下单用户
        /// </summary>
        [MaxLength(DbConst.KeyLength)]
        public string UserId { get; set; }

        /// <summary>
        /// 取餐码
        /// </summary>
        [MaxLength(DbConst.Length_10)]
        public string FetchNumber { get; set; }

        /// <summary>
        /// 拒单原因
        /// </summary>
        [MaxLength(DbConst.Length_100)]
        public string RefusedReason { get; set; }

        [ForeignKey(nameof(SeatId))]
        public virtual Seat Seat { get; set; }

        [ForeignKey(nameof(BillId))]
        public virtual Bill Bill { get; set; }

        [ForeignKey(nameof(CenterId))]
        public virtual HawkerCenter Center { get; set; }

        [ForeignKey(nameof(RestaurantId))]
        public virtual Restaurant Restaurant { get; set; }

        public virtual IList<Order_Dish> Order_Dishes { get; set; }

        public DateTime? LastModifiedTime { get; set; }

        public DateTime CreatedTime { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }
    }
}
