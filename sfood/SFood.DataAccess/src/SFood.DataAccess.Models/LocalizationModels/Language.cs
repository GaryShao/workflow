using SFood.DataAccess.Common.Consts;
using SFood.DataAccess.Models.Infrastructure;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SFood.DataAccess.Models.LocalizationModels
{
    [Table("Languages", Schema = "Localization")]
    public class Language : UuidEnity
    {
        [MaxLength(DbConst.Length_20)]
        public string Code { get; set; }
    }
}
