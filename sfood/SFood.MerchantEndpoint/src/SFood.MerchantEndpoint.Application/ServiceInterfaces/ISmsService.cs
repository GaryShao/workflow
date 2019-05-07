using SFood.MerchantEndpoint.Application.Dtos.Parameters.Account;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Application
{
    public interface ISmsService
    {
        Task SendVerficationCode(SendVCodeSMSParam param);

        Task<double?> GetElapsedTimeOfLatestCode(string phone);
    }
}
