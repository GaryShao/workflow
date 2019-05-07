using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Common.Providers.Interfaces
{
    public interface IResourceProvider
    {
        Task<IDictionary<string, string>> Get(string key, string languageId);
    }
}
