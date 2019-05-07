using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFood.DataAccess.Infrastructure.Interfaces;
using SFood.DataAccess.Models.LocalizationModels;
using SFood.MerchantEndpoint.Common.Providers.Interfaces;

namespace SFood.MerchantEndpoint.Common.Providers.Implements
{
    public class PersistedResourceProvider// : IResourceProvider
    {
        private readonly IReadOnlyRepository _readonlyRepository;

        public PersistedResourceProvider(IReadOnlyRepository readOnlyRepository)
        {
            _readonlyRepository = readOnlyRepository;
        }

        public async Task<IDictionary<string, string>> GetResources()
        {
            var resources = await _readonlyRepository.GetAllAsync<Resource>();
            return resources.Select(r => 
                new KeyValuePair<string, string>($"{r.LanguageId}::{r.Key}", r.Value)).ToDictionary(
                p => p.Key, p => p.Value);
        }
    }
}
