using SFood.DataAccess.Common.Consts;
using System.ComponentModel.DataAnnotations;

namespace SFood.DataAccess.Models.Infrastructure
{
    public abstract class UuidEnity: Entity<string>
    {
        [MaxLength(DbConst.KeyLength)]
        public new string Id { get; set; }
    }
}
