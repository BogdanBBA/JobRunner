using CommonCode.Utils;
using System;

namespace Jobs.Anniversaries.Classes
{
    /// <summary>
    /// Duration information container, similar to the TimeSpan class, but for dates.
    /// </summary>
    public class DateSpan
    {
        public DateTime DateA { get; private set; }
        public DateTime DateB { get; private set; }
        public int Years { get; private set; }
        public int MonthsInYear { get; private set; }
        public int DaysInMonth { get; private set; }
        public int TotalMonths { get; private set; }
        public int TotalWeeks { get; private set; }
        public int TotalDays { get; private set; }

        public DateSpan()
        {
            // initialize values
            DateA = DateB = DateTime.Now.Date;
            Years = TotalMonths = TotalWeeks = TotalDays = MonthsInYear = DaysInMonth = 0;
        }

        public DateSpan(DateTime a, DateTime b)
            : this()
        {
            // swap if they're anti-chronological, and initialize date fields
            if (a.CompareTo(b) > 0)
                Utils.Swap(ref a, ref b);
            DateA = a;
            DateB = b;

            // gets total days by calculating from start, day-by-day, until end is reached; then calculate total weeks by division
            while (DateA.AddDays(TotalDays + 1).CompareTo(DateB) <= 0)
                TotalDays++;
            TotalWeeks = TotalDays / 7;

            // get total months by calculating from start, month-by-month, until end is reached
            // AddMonths() is better than manually adding the amount of days in that month, in cases where the day-of-month of start date is not valid in other months
            // for example, 30 Jan: adding 31 days (Jan) would yield 2 Feb; whereas AddMonth() would yield 28 Feb -- arbitrarily preferred
            // calculation from start date is used to avoid the alterations caused by cases such as the one above (30 Jan - 28 Feb - 28 Mar - 28 Apr ...)
            while (DateA.AddMonths(TotalMonths + 1).CompareTo(DateB) <= 0)
                TotalMonths++;
            MonthsInYear = TotalMonths % 12;

            // get years by calculating from start, year-by-year, until end is reached
            while (DateA.AddYears(Years + 1).CompareTo(DateB) <= 0)
                Years++;

            // get days-in-month by difference in days calculation between end dates obtained by multiplication of total months / total days
            DaysInMonth = (int)Math.Floor(DateA.AddDays(TotalDays).Subtract(DateA.AddMonths(TotalMonths)).TotalDays);
        }
    }
}
