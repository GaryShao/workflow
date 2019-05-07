using Microsoft.AspNetCore.Identity;
using SFood.DataAccess.Common.Consts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFood.DataAccess.Models.IdentityModels
{
    [Table("RoleClaims", Schema = "IdentitySchema")]
    public class RoleClaim : IdentityRoleClaim<string>
    {
        [MaxLength(DbConst.KeyLength)]
        public override string RoleId { get => base.RoleId; set => base.RoleId = value; }
    }
}
