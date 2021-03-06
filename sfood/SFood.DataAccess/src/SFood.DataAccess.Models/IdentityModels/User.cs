﻿using Microsoft.AspNetCore.Identity;
using SFood.DataAccess.Common.Consts;
using SFood.DataAccess.Models.Infrastructure.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFood.DataAccess.Models.IdentityModels
{
    [Table("Users", Schema = "IdentitySchema")]
    public class User : IdentityUser<string>, IEntity<string>
    {
        [MaxLength(DbConst.KeyLength)]
        public override string Id { get => base.Id; set => base.Id = value; }

        object IEntity.Id { get => Id; set => Id = (string)value; }
    }
}
