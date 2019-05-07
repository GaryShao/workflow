using SFood.DataAccess.Common.Consts;
using SFood.DataAccess.Common.Enums;
using SFood.DataAccess.Models.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFood.DataAccess.Models.IdentityModels
{
    [Table("UserExtensions", Schema = "IdentitySchema")]
    public class UserExtension : UuidEnity
    {
        [MaxLength(DbConst.KeyLength)]
        public string UserId { get; set; }

        [MaxLength(DbConst.KeyLength)]
        public string CountryCodeId { get; set; }

        public StaffStatus? StaffStatus { get; set; }

        [MaxLength(DbConst.Length_100)]
        public string NickName { get; set; }

        [MaxLength(DbConst.KeyLength)]
        public string RestaurantId { get; set; }      

        public DateTime? LastLoginTime { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        [ForeignKey(nameof(RestaurantId))]
        public virtual Restaurant Restaurant { get; set; }
    }
}
