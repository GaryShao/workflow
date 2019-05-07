using System.Collections.Generic;

namespace SFood.MerchantEndpoint.Common.Utilities
{
    public interface ISecurityUtility
    {
        string GenerateSign(Dictionary<string, string> dictionary);

        string Generate(int size);
    }
}
