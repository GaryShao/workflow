using SFood.DataAccess.Common.Consts;
using SFood.DataAccess.Models.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFood.DataAccess.Models.LocalizationModels
{
    [Table("Resources", Schema = "Localization")]
    public class Resource : UuidEnity
    {
        [MaxLength(DbConst.Length_100)]
        public string Key { get; set; }

        [MaxLength(DbConst.Length_500)]
        public string Value { get; set; }

        [MaxLength(DbConst.KeyLength)]
        public string LanguageId { get; set; }
    }
}
