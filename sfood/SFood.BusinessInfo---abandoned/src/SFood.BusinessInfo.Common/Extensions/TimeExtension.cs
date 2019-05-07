namespace SFood.BusinessInfo.Common.Extensions
{
    public static class TimeExtension
    {
        public static string ToStringTime(this short integerTime)
        {            
            var hour = integerTime / 60;
            var mins = integerTime % 60;

            var hourString = hour.ToString();
            var minString = mins.ToString();

            if (hour < 10)
            {
                hourString = "0" + hourString;
            }

            if (mins < 10)
            {
                minString = "0" + minString;
            }

            return $"{hourString}:{minString}";
        }
    }
}
