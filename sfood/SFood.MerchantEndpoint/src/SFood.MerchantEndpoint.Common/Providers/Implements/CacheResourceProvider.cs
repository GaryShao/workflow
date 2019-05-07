using SFood.MerchantEndpoint.Common.Providers.Interfaces;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Common.Providers.Implements
{
    public class CacheResourceProvider// : ICacheResourceProvider
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public CacheResourceProvider(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }

        public Task<IDictionary<string, string>> GetResources()
        {
            throw new NotImplementedException();
        }

        public void Insert(KeyValuePair<string, string> pair)
        {
            throw new NotImplementedException();
        }
    }
}
