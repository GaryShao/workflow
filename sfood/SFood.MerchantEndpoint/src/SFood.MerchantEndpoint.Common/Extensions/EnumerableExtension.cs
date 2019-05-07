using System;
using System.Collections.Generic;

namespace SFood.MerchantEndpoint.Common.Extensions
{
    public static class EnumerableExtension
    {
        public static void ForEach<T>(this IEnumerable<T> @this, Action<T> action)
        {
            foreach (T item in @this)
            {
                action(item);
            }
        }
    }
}
