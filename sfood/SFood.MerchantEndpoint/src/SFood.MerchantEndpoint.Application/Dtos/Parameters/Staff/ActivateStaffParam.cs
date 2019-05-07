using SFood.MerchantEndpoint.Common.Consts;
using System.ComponentModel.DataAnnotations;

namespace SFood.MerchantEndpoint.Application.Dtos.Parameters.Staff
{
    public class ActivateStaffParam
    {
        [Required]
        [RegularExpression(AppConsts.PasswordRegex, ErrorMessage =
            "Password must contain number, capital or lower-case letter, symbol; more than 6 and less than 12 letters")]
        public string Phone { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
