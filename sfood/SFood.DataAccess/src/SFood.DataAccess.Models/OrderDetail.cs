using SFood.DataAccess.Common.Consts;
using SFood.DataAccess.Models.Infrastructure;
using SFood.DataAccess.Models.Infrastructure.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFood.DataAccess.Models
{
    [Table("Details", Schema = "OrderInfo")]
    public class OrderDetail : UuidEnity, IHasVersion
    {
        [MaxLength(DbConst.KeyLength)]
        public string OrderId { get; set; }

        [Timestamp]
        public byte[] Version { get; set; }

        public DateTime? PendingToCooking { get; set; }

        public DateTime? CookingToDeliveOrTaking { get; set; }

        public DateTime? DeliveOrTakingToDone { get; set; }

        public DateTime? AnyToClosed { get; set; }
    }
}
