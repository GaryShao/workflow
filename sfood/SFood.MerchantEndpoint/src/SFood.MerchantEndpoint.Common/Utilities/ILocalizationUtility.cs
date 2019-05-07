using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SFood.MerchantEndpoint.Common.Utilities
{
    public interface ILocalizationUtility
    {
        IDictionary<string, string> GetResources();
    }
}
