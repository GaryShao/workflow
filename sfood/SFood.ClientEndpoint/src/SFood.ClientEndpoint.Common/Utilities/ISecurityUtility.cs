using System.Collections.Generic;

namespace SFood.ClientEndpoint.Common.Utilities
{
    public interface ISecurityUtility
    {
        string GenerateSign(Dictionary<string, string> dictionary);

        string Generate(int size);
    }
}
