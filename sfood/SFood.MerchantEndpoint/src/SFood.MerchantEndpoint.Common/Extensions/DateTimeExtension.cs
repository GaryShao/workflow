using System;
using System.Globalization;

namespace SFood.MerchantEndpoint.Common.Extensions
{
    public static class DateTimeExtension
    {
        public static string TimeDescription(this DateTime time)
        {
            var elapsed = DateTime.UtcNow - time;

            var elapsedHour = elapsed.TotalHours;

            if (elapsedHour > 1)
            {
                return $"{Convert.ToInt32(elapsedHour)} hours ago";
            }
            else
            {
                var elapsedMins = elapsed.TotalMinutes;

                if (elapsedMins > 2)
                {
                    return $"{Convert.ToInt32(elapsedMins)} minutes ago";
                }
                else
                {
                    var elapsedSeconds = elapsed.TotalSeconds;
                    if (elapsedSeconds <= 60)
                    {
                        return "just now";
                    }
                    else
                    {
                        return "1 minute ago";
                    }
                }
            }
        }

        /// <summary>
        /// Turn datetime to simple date format
        /// <para>such as: 2018/11/15 16:32 would be '20181115'</para> 
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string ToSimpleDate(this DateTime time)
        {
            return time.Date.ToString("yyyyMMdd", DateTimeFormatInfo.InvariantInfo);
        }
    }
}
