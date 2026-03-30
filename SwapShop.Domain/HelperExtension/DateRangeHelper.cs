using SwapShop.Domain.Enum;


namespace SwapShop.Domain.HelperExtension
{
    public static class DateRangeHelper
    {
        public static (DateTime currentStart, DateTime prevStart, DateTime prevEnd) GetDateRanges(PeriodicFilter filter)
        {
            DateTime now = DateTime.UtcNow;
            DateTime currentStart, prevStart, prevEnd;

            switch (filter)
            {
                case PeriodicFilter.Today:
                    currentStart = now.Date;
                    prevStart = currentStart.AddDays(-1);
                    prevEnd = currentStart;
                    break;

                case PeriodicFilter.ThisWeek:
                    currentStart = now.Date.AddDays(-(int)now.DayOfWeek);
                    prevStart = currentStart.AddDays(-7);
                    prevEnd = currentStart;
                    break;
                case PeriodicFilter.ThisMonth:
                    currentStart = DateTime.SpecifyKind(new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc), DateTimeKind.Utc);
                    prevStart = currentStart.AddMonths(-1);
                    prevEnd = currentStart;
                    break;
                /*case PeriodicFilter.ThisMonth:
                    currentStart = new DateTime(now.Year, now.Month, 1);
                    prevStart = currentStart.AddMonths(-1);
                    prevEnd = currentStart;
                    break;*/
                case PeriodicFilter.AllTime:
                default:
                    currentStart = DateTime.MinValue;
                    prevStart = DateTime.MinValue;
                    prevEnd = DateTime.MinValue;
                    break;
            }

            return (currentStart, prevStart, prevEnd);
        }

    }
}
