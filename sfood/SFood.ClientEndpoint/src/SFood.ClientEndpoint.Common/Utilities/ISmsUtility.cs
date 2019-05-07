using SFood.ClientEndpoint.Common.Dtos.CommsHub;
using System.Threading.Tasks;

namespace SFood.ClientEndpoint.Common.Utilities
{
    public interface ISmsUtility
    {
        Task SendingAsync(SendParam param);
    }
}
