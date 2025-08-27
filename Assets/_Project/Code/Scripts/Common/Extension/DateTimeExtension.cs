using System;

namespace NamPhuThuy.Common
{
    public static class DateTimeExtension
    {
        public static bool IsDayAfter(this DateTime current, DateTime compare)
        {
            if (current.Year > compare.Year)
            {
                return true;
            }
            else if (current.Year == compare.Year)
            {
                if (current.Month > compare.Month) return true;
                else if (current.Month == compare.Month)
                {
                    return current.Day > compare.Day;
                }
            }
            return false;
        }

        public static string GetHourFormat(this float second)
        {
            return GetHourFormat((int)second);
        }

        public static string GetHourFormat(this int second)
        {
            int value = second;
            int secondLeft = value % TimeConst.SECONDS_PER_HOUR;
            int hour = value / TimeConst.SECONDS_PER_HOUR;
            string hourFormat = hour.ToString().FillBefore("0", 2);
            return hourFormat + ":" + GetMinuteFormat(secondLeft);
        }
        public static string ToMinuteFormat(this float second)
        {
            return GetMinuteFormat((int)second);
        }
        public static string GetMinuteFormat(this int second)
        {
            int minute = second / TimeConst.SECOND_PER_MINUTE;
            second = second % TimeConst.SECOND_PER_MINUTE;
            string minuteFormat = minute.ToString().FillBefore("0", 2);
            string secondFormat = second.ToString().FillBefore("0", 2);
            return minuteFormat + ":" + secondFormat;
        }
    }
}

public class TimeConst
{
    public const int SECONDS_PER_HOUR = 3600;

    public const int SECONDS_PER_DAY = 86400;

    public const int SECOND_PER_MINUTE = 60;

    public const int MINUTES_PER_HOUR = 60;

    public const int MINUTES_PER_DAY = 1440;

    public const int HOURS_PER_DAY = 24;
}