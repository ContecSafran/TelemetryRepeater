using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkUtil
{
    public class TimeUtil
    {

        public static DateTime GetLapsedTime(string utc)
        {
            string timeString = string.Empty;

            return DateTime.ParseExact(utc, "yyyy-MM-dd'T'HH:mm:ss'Z'",
                                           CultureInfo.InvariantCulture,
                                           DateTimeStyles.AssumeUniversal |
                                           DateTimeStyles.AdjustToUniversal);

        }

        public static string GetLapsedTime(DateTime dateTime)
        {
            TimeSpan ts = DateTime.UtcNow.Subtract(dateTime);

            int DayPeriod = Math.Abs(ts.Days);

            if (DayPeriod < 1)
            {
                int HourPeriod = Math.Abs(ts.Hours);

                if (HourPeriod < 1)
                {
                    int MinutePeriod = Math.Abs(ts.Minutes);
                    if (MinutePeriod < 1)
                    {
                        int SecondPeriod = Math.Abs(ts.Seconds);
                        return " * " + SecondPeriod.ToString().PadLeft(2, '\u2000') + "초전";
                    }
                    else
                    {
                        return " * " + MinutePeriod.ToString().PadLeft(2, '\u2000') + "분전";
                    }
                }
                else
                {
                    return " - " + HourPeriod.ToString().PadLeft(2, '\u2000') + "시간전";
                }
            }
            else if ((DayPeriod > 0) && (DayPeriod < 7))
            {
                return " ? " + DayPeriod.ToString().PadLeft(2, '\u2000') + "일전";
            }
            else if (DayPeriod == 7)
            {
                return " ? " + "1".PadLeft(2, '\u2000') + "주전";
            }
            else
            {
                return dateTime.ToString("yyyy년 MM월 dd일");
            }
        }
    }
}
