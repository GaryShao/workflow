namespace SFood.ClientEndpoint.Common.Extensions
{
    public static class DecimalExtension
    {
        public static string ToMoneyString(this decimal money)
        {
            return money.ToString("#,##0.00");
        }
    }
}
