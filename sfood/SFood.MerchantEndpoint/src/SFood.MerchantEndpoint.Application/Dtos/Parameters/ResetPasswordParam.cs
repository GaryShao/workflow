using SFood.MerchantEndpoint.Common.Consts;
using System.ComponentModel.DataAnnotations;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters
{
    public class ResetPasswordParam
    {
        public string UserId { get; set; }

        [Required]
        [RegularExpression(AppConsts.PasswordRegex)]
        public string OldPassword { get; set; }

        [Required]
        [RegularExpression(AppConsts.PasswordRegex)]
        public string NewPassword { get; set; }
    }
}
