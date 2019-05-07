using System;
using System.Collections.Generic;

namespace SFood.MerchantEndpoint.Common.Comparers
{
    /// <summary>
    /// ASCII值排序
    /// </summary>
    public class OrdinalComparer : IComparer<String>
    {
        public int Compare(String x, String y)
        {
            return string.CompareOrdinal(x, y);
        }
    }
}
