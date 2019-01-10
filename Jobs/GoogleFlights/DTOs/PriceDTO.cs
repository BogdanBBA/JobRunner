using CommonCode;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text.RegularExpressions;

namespace Jobs.GoogleFlights.DTOs
{
    public class PriceDTO : BaseDTO
    {
        public const string TIMES_REGEX = "(\\d+:\\d+) ?\\- ?(\\d+:\\d+)";

        public long? RouteID { get; set; }
        public DateTime Moment { get; set; }
        public string FromAirport { get; set; }
        public string ToAirport { get; set; }
        public TimeSpan TakeOffTime { get; set; }
        public TimeSpan LandingTime { get; set; }
        public TimeSpan FlightDuration { get; set; }
        public string Company { get; set; }
        public string Layovers { get; set; }
        public long Price { get; set; }
        public string Currency { get; set; }

        public static List<PriceDTO> ParseList(SQLiteDataReader reader)
        {
            List<PriceDTO> result = new List<PriceDTO>();
            while (reader.Read())
            {
                result.Add(ParseCurrent(reader));
            }
            return result;
        }

        public static PriceDTO ParseCurrent(SQLiteDataReader reader)
        {
            return new PriceDTO()
            {
                RouteID = reader.GetInt64(0),
                Moment = reader.GetDateTime(1),
                FromAirport = reader.GetString(2),
                ToAirport = reader.GetString(3),
                TakeOffTime = reader.GetDateTime(4).TimeOfDay,
                LandingTime = reader.GetDateTime(5).TimeOfDay,
                FlightDuration = reader.GetDateTime(6).TimeOfDay,
                Company = reader.GetString(7),
                Layovers = reader.GetString(8),
                Price = reader.GetInt64(9),
                Currency = reader.GetString(10)
            };
        }

        public static PriceDTO FromGoogleFlightsStrings(List<string> lines)
        {
            int length = lines.Count;
            Match match = Regex.Match(lines[0], TIMES_REGEX);
            TimeSpan takeOff = TimeSpan.Parse(match.Groups[1].Value);
            TimeSpan landing = TimeSpan.Parse(match.Groups[2].Value);
            ParsePriceAndCurrency(lines[length - 1], out long price, out string currency);
            string layovers = lines[length - 2];
            string fromAirport = lines[length - 3].Split('–')[0];
            string toAirport = lines[length - 3].Split('–')[1];
            match = Regex.Match(lines[length - 4], "(\\d+)[^\\d]+(\\d+)[^\\d]+");
            TimeSpan flightDuration = new TimeSpan(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value), 0);
            List<string> companies = new List<string>();
            for (int index = 1; index <= length - 5; index++)
                companies.Add(lines[index]);

            return new PriceDTO()
            {
                RouteID = null,
                Moment = DateTime.Now,
                FromAirport = fromAirport,
                ToAirport = toAirport,
                TakeOffTime = takeOff,
                LandingTime = landing,
                FlightDuration = flightDuration,
                Company = string.Join("/", companies),
                Layovers = layovers,
                Price = price,
                Currency = currency
            };
        }

        private static void ParsePriceAndCurrency(string text, out long price, out string currency)
        {
            if (text.Contains(",") && text.Contains("."))
                text = text.Replace(",", "");
            /*bool again = true;
            do
            {
                Match match = Regex.Match("\\.(\\d\\{3\\})", text);
                if (match.Success)
                    text = text.Replace(match.Value, match.Groups[1].Value);
                again = match.Success;
            } while (again);*/
            text = text.Replace(".", "");
            string[] parts = text.Replace(",", ".").Trim().Split(' ');
            if (!Regex.IsMatch(parts[0], "^[\\d\\.]+$"))
                Utils.Swap(ref parts[0], ref parts[1]);
            price = (long)Math.Ceiling(double.Parse(parts[0]));
            currency = parts[1];
        }

        public override string ToSQL
            => $"{RouteID?.ToString() ?? "NULL"}, {Moment.ToSqlDateTime()}, '{FromAirport}', '{ToAirport}', {TakeOffTime.ToSqlTime()}, {LandingTime.ToSqlTime()}, {FlightDuration.ToSqlTime()}, '{Company}', '{Layovers}', {Price}, '{Currency}'";
    }
}
