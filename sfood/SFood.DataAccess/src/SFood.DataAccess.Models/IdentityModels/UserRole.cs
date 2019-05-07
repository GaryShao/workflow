using Microsoft.AspNetCore.Identity;
using SFood.DataAccess.Common.Consts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFood.DataAccess.Models.IdentityModels
{
    [Table("UserRoles", Schema = "IdentitySchema")]
    public class UserRole : IdentityUserRole<string>
    {
        [MaxLength(DbConst.KeyLength)]
        public override string UserId { get => base.UserId; set => base.UserId = value; }

        [MaxLength(DbConst.KeyLength)]
        public override string RoleId { get => base.UserId; set => base.UserId = value; }
    }
}
