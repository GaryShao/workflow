﻿using Microsoft.AspNetCore.Identity;
using SFood.DataAccess.Common.Consts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFood.DataAccess.Models.IdentityModels
{
    [Table("UserTokens", Schema = "IdentitySchema")]
    public class UserToken : IdentityUserToken<string>
    {
        [MaxLength(DbConst.KeyLength)]
        public override string UserId { get => base.UserId; set => base.UserId = value; }
    }
}
