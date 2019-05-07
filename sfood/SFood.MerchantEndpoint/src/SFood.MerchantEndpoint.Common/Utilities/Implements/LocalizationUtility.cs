using SFood.MerchantEndpoint.Common.Providers.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Common.Utilities.Implements
{
    public class LocalizationUtility : ILocalizationUtility
    {
        private readonly IResourceProvider _resourceProvider;

        public LocalizationUtility(IResourceProvider resourceProvider)
        {
            _resourceProvider = resourceProvider;            
        }

        /// <summary>
        /// format:        
        /// <para>Key: LanguageId::ResourceKey</para>
        /// <para>Value: ResourceValue</para>
        /// </summary>
        public IDictionary<string, string> GetResources()
        {
            //return await _resourceProvider.GetResources();
            throw new System.NotImplementedException();
        }
    }
}
