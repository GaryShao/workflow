using SFood.MerchantEndpoint.Application.Dtos.Parameters;
using SFood.MerchantEndpoint.Application.Dtos.Parameters.Account;
using SFood.MerchantEndpoint.Application.Dtos.Results.Account;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Application
{
    public interface IAccountService
    {
        Task UpdatePasswordByPhone(RetrievePasswordParam param);

        Task UpdatePasswordByUserId(ResetPasswordParam param);

        Task<bool> IsPassworRight(VerificationPasswordParam param);

        Task UpdatePhone(UpdatePhoneNumberParam param);

        Task<bool> IsVerificationCodeValid(VerificationCodeValidationParam param);

        Task Logout(string userId);

        Task<List<GroupedCountryCodesResult>> GetDialingCodes();
    }
}
