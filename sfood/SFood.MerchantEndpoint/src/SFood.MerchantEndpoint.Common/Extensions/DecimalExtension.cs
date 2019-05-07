namespace SFood.MerchantEndpoint.Common.Extensions
{
    public static class DecimalExtension
    {
        public static string ToMoneyString(this decimal money)
        {
            return money.ToString("F2");
        }
    }
}
