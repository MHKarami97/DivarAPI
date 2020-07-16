using System;

namespace Common.Utilities
{
    public static class DateExtensions
    {
        public static string ToBeautyDate(this DateTimeOffset date)
        {
            var time = "بیشتر از 1 سال";

            var now = DateTimeOffset.Now;

            if (date - now < new TimeSpan(0, 1, 0, 0) &&
                date - now > new TimeSpan(0, 0, 0, 0))
            {
                time = "کمتر از 1 ساعت";
            }
            else if (date - now < new TimeSpan(0, 12, 0, 0) &&
                     date - now > new TimeSpan(0, 1, 0, 0))
            {
                time = "کمتر از 12 ساعت";
            }
            else if (date - now > new TimeSpan(1, 0, 0, 0) &&
                     date - now < new TimeSpan(1, 23, 0, 0))
            {
                time = "1 روز قبل";
            }
            else if (date - now < new TimeSpan(7, 0, 0, 0) &&
                     date - now > new TimeSpan(6, 0, 0, 0))
            {
                time = "کمتر از 1 هفته";
            }
            else if (date - now < new TimeSpan(30, 0, 0, 0) &&
                     date - now > new TimeSpan(7, 0, 0, 0))
            {
                time = "کمتر از 1 ماه";
            }
            else if (date - now > new TimeSpan(30, 0, 0, 0) &&
                     date - now < new TimeSpan(180, 0, 0, 0))
            {
                time = "کمتر از 6 ماه";
            }
            else if (date - now > new TimeSpan(180, 0, 0, 0) &&
                     date - now < new TimeSpan(360, 0, 0, 0))
            {
                time = "کمتر از 1 سال";
            }

            return time;
        }
    }
}
