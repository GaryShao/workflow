using System;

namespace SFood.Auth.Common.Extensions
{
    public static class GuidExtension
    {
        public static string ToUuidString(this Guid guid)
        {
            var original = guid.ToString();
            return original.Replace("-", "");
        }
    }
}
