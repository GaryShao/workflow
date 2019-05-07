namespace SFood.BusinessInfo.Common.Extensions
{
    public static class StringExtension
    {
        public static bool IsNullOrWhiteSpace(this string src)
        {
            return string.IsNullOrWhiteSpace(src);
        }
    }
}
