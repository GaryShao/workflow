using SFood.DataAccess.Common.Consts;
using SFood.DataAccess.Models.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFood.DataAccess.Models
{
    /// <summary>
    /// dialing code for variable countries
    /// </summary>
    [Table("CountryCodes", Schema = "Common")]
    public class CountryCode : UuidEnity
    {
        [MaxLength(DbConst.Length_20)]
        public string Code { get; set; }

        [MaxLength(DbConst.Length_100)]
        public string Name { get; set; }

        [MaxLength(DbConst.Length_100)]
        public string EnglishName { get; set; }

        [MaxLength(DbConst.Length_500)]
        public string FlagUrl { get; set; }
    }
}
