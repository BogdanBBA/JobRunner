using CommonCode;
using Jobs.Anniversaries.Classes;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Jobs.Anniversaries
{
    public class AnniversaryDTO
    {
        /// <summary>Defines the possible types of anniversaries.</summary>
        public enum Categories { MetSomeone = 0, Birthday = 1, FirstTimeDidSomething = 2, Romantic = 3 };
        /// <summary>Defines the possible anniversary celebration frequencies.</summary>
        public enum CelebrationFrequencies { OnlyOnce = 0, Daily = 1, Weekly = 2, Monthly = 3, Yearly = 4 };

        /// <summary></summary>
        public string Title { get; set; }
        /// <summary></summary>
        public Categories Category { get; set; }
        /// <summary></summary>
        public DateTime OriginalEvent { get; set; }
        /// <summary></summary>
        public CelebrationFrequencies Frequency { get; set; }
        /// <summary></summary>
        public long HeadsUpInDays { get; set; }

        /// <summary>Determines the next anniversary date for the current event after the given 'today' date, depending on the specified celebration frequency.</summary>
        public DateTime NextAnniversary(DateTime today)
        {
            DateTime date = OriginalEvent.Date;
            while (date.CompareTo(today) < 0)
                switch (Frequency)
                {
                    case CelebrationFrequencies.OnlyOnce:
                        date = OriginalEvent.Date.CompareTo(today) >= 0 ? OriginalEvent.Date : DateTime.MaxValue;
                        break;
                    case CelebrationFrequencies.Daily:
                        date = date.AddDays(1);
                        break;
                    case CelebrationFrequencies.Weekly:
                        date = date.AddDays(7);
                        break;
                    case CelebrationFrequencies.Monthly:
                        date = date.AddMonths(1);
                        break;
                    case CelebrationFrequencies.Yearly:
                        date = date.AddYears(1);
                        break;
                    default:
                        date = DateTime.MaxValue;
                        break;
                }
            return date;
        }

        /// <summary>Determines, depending on the specified celebration frequency, whether the given 'today' date is an anniversary of the given 'anniversary' date.</summary>
        public bool AnniversaryIsToday(DateTime anniversary, DateTime today)
        {
            switch (Frequency)
            {
                case CelebrationFrequencies.OnlyOnce:
                    return today.Date.Equals(OriginalEvent.Date);
                case CelebrationFrequencies.Daily:
                    return true;
                case CelebrationFrequencies.Weekly:
                    return today.DayOfWeek == anniversary.DayOfWeek;
                case CelebrationFrequencies.Monthly:
                    return today.Day == anniversary.Day;
                case CelebrationFrequencies.Yearly:
                    return today.Day == anniversary.Day && today.Month == anniversary.Month;
                default:
                    return false;
            }
        }

        /// <summary>Determines whether a reminder should be sent (indifferent of whether the anniversary is the given 'today' date, or the specified amount of heads-up days away from 'today').</summary>
        public bool ShouldSendReminder(DateTime today)
        {
            DateTime anniversary = NextAnniversary(today);
            if (AnniversaryIsToday(anniversary, today))
                return true;
            return AnniversaryIsToday(anniversary.AddDays(-HeadsUpInDays), today);
        }

        /// <summary></summary>
        public string FormatAnniversaryAge(DateTime anniversary, DateTime today)
        {
            if (!AnniversaryIsToday(anniversary, today) && !AnniversaryIsToday(anniversary.AddDays(-HeadsUpInDays), today))
                return "FormatAnniversaryAge WARNING: today is not an anniversary or a heads-up date (this: " + this + ")";

            DateSpan diff = new DateSpan(anniversary, OriginalEvent.Date);

            switch (Frequency)
            {
                case CelebrationFrequencies.OnlyOnce:
                    return "one-off event";
                case CelebrationFrequencies.Daily:
                    return Utils.Plural("day", diff.TotalDays, true);
                case CelebrationFrequencies.Weekly:
                    return Utils.Plural("week", diff.TotalWeeks, true);
                case CelebrationFrequencies.Monthly:
                    return string.Format("{0} ({1})",
                        Utils.Plural("month", diff.TotalMonths, true),
                        diff.TotalMonths % 12 == 0
                            ? string.Format("{0}", Utils.Plural("year", diff.Years, true))
                            : string.Format("{0} {1}", Utils.Plural("year", diff.Years, true), Utils.Plural("month", diff.MonthsInYear, true)));
                case CelebrationFrequencies.Yearly:
                    return Utils.Plural("year", diff.Years, true);
                default:
                    return "FormatAnniversaryAge WARNING: invalid celebration frequency (this: " + this + ")";
            }
        }

        public static List<AnniversaryDTO> ParseList(SQLiteDataReader reader)
        {
            List<AnniversaryDTO> result = new List<AnniversaryDTO>();
            while (reader.Read())
            {
                result.Add(ParseCurrent(reader));
            }
            return result;
        }

        public static AnniversaryDTO ParseCurrent(SQLiteDataReader reader)
        {
            return new AnniversaryDTO()
            {
                Title = reader.GetString(0),
                Category = (Categories)((int)reader.GetInt64(1)),
                OriginalEvent = reader.GetDateTime(2),
                Frequency = (CelebrationFrequencies)((int)reader.GetInt64(3)),
                HeadsUpInDays = reader.GetInt64(4)
            };
        }

        public override string ToString()
            => $"{OriginalEvent:yyyy-MM-dd} ({Frequency})";
    }
}
