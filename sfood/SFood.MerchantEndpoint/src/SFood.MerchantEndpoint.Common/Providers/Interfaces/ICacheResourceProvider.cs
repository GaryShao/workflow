using System.Collections.Generic;

namespace SFood.MerchantEndpoint.Common.Providers.Interfaces
{
    public interface ICacheResourceProvider : IResourceProvider
    {
        void Insert(KeyValuePair<string, string> pair);
    }
}
