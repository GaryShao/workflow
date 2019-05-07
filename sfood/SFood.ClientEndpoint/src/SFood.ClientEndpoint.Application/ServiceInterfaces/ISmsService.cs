using SFood.ClientEndpoint.Application.Dtos.Parameter;
using System.Threading.Tasks;

namespace SFood.ClientEndpoint.Application.ServiceInterfaces
{
    public interface ISmsService
    {
        Task SendVerficationCode(SendVCodeSMSParam param);

        Task<double?> GetElapsedTimeOfLatestCode(string phone);
    }
}
