using CommonCode;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Jobs.GoogleFlights.DTOs
{
    public class FlightDTO : BaseDTO
    {
        public const string TIMES_REGEX = "(\\d+:\\d+) ?\\- ?(\\d+:\\d+)";

        public long? RecordingID { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public TimeSpan TakeOff { get; set; }
        public TimeSpan Landing { get; set; }
        public TimeSpan FlightDuration { get; set; }
        public string Company { get; set; }
        public string Layovers { get; set; }
        public string Price { get; set; }

        public static FlightDTO FromStrings(List<string> lines)
        {
            int index = lines.Count;
            Match match = Regex.Match(lines[0], TIMES_REGEX);
            TimeSpan takeOff = TimeSpan.Parse(match.Groups[1].Value);
            TimeSpan landing = TimeSpan.Parse(match.Groups[2].Value);
            string price = lines[index - 1];
            string layovers = lines[index - 2];
            string from = lines[index - 3].Split('–')[0];
            string to = lines[index - 3].Split('–')[1];
            match = Regex.Match(lines[index - 4], "(\\d+)[^\\d]+(\\d+)[^\\d]+");
            TimeSpan flightDuration = new TimeSpan(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value), 0);
            List<string> companies = new List<string>();
            for (index = 2; index <= index - 5; index++)
                companies.Add(lines[index]);

            return new FlightDTO()
            {
                RecordingID = null,
                From = from,
                To = to,
                TakeOff = takeOff,
                Landing = landing,
                FlightDuration = flightDuration,
                Company = string.Join("/", companies),
                Layovers = layovers,
                Price = price
            };
        }

        public override string ToSQL
            => $"{RecordingID?.ToString()??"NULL"}, '{From}', '{To}', {TakeOff.ToSqlTime()}, {Landing.ToSqlTime()}, {FlightDuration.ToSqlTime()}, '{Company}', '{Layovers}', '{Price}'";
    }
}
