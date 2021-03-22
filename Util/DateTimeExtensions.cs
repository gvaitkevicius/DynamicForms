using System;

namespace DynamicForms.Util
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime EndOfWeek(this DateTime dt)
        {
            DateTime ndt = dt.StartOfWeek(DayOfWeek.Sunday).AddDays(6);
            return new DateTime(ndt.Year, ndt.Month, ndt.Day, 23, 59, 59, 999);
        }
    }
}
