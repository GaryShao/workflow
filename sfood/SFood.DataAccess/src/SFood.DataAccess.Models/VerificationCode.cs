using SFood.DataAccess.Common.Consts;
using SFood.DataAccess.Models.Infrastructure;
using SFood.DataAccess.Models.Infrastructure.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFood.DataAccess.Models
{
    [Table("VerificationCodes", Schema = "Common")]
    public class VerificationCode : UuidEnity, IHasCreatedTime
    {
        [MaxLength(DbConst.Length_10)]
        public string Code { get; set; }

        [MaxLength(DbConst.Length_20)]
        public string Phone { get; set; }        

        public DateTime CreatedTime { get; set; }
    }
}
