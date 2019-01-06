using CommonCode;
using DataLayer;
using Jobs.OpenWeather.JsonDTOs;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Jobs.OpenWeather.DTOs
{
    public class WeatherStateDTO : BaseDTO
    {
        /// <summary>
        /// C# DateTime(long ticks): A date and time expressed in the number of 100-nanosecond intervals that have elapsed 
        /// since January 1, 0001 at 00:00:00.000 in the Gregorian calendar.<para/>
        /// Java Date(long milliseconds): Allocates a Date object and initializes it to represent the specified number of milliseconds 
        /// since the standard base time known as "the epoch", namely January 1, 1970, 00:00:00 GMT.
        /// </summary>
        private static readonly DateTime DOTNET_JAVA_DATE_DIFFERENCE = new DateTime(1970, 1, 1, 3, 0, 0);

        public long CityID { get; set; }
        public DateTime Moment { get; set; }
        public string Description { get; set; }
        public double Temperature { get; set; }
        public double Cloudiness { get; set; }
        public double WindSpeed { get; set; }
        public long AtmosphericPressure { get; set; }
        public long Humidity { get; set; }

        private static DateTime GetRomaniaTime(DateTime dt)
        {
            // round to minute
            dt = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);
            // EEST=UTC+3 overrides EET=UTC+2 between the last Sundays in March/October; so, add either 2 or 3 hours to the value
            return TimeZoneInfo.ConvertTimeFromUtc(dt, TimeZoneInfo.FindSystemTimeZoneById("GTB Standard Time"));
        }

        public static WeatherStateDTO FromCityResult(CityResult jsonDTO)
        {
            // DateTime updated = GetRomaniaTime(DateTimeOffset.FromUnixTimeSeconds(jsonDTO.Dt).DateTime);
            DateTime updated = DateTimeOffset.FromUnixTimeSeconds(jsonDTO.Dt).DateTime;
            return new WeatherStateDTO()
            {
                CityID = jsonDTO.Id,
                Moment = updated, 
                Description = jsonDTO.Weather[0].Description, // example: "clear sky"
                Temperature = jsonDTO.Main.Temp - 273.15, // default: Kelvin
                Cloudiness = jsonDTO.Clouds.All / 100.0, // default: 0-100%
                WindSpeed = jsonDTO.Wind.Speed, // default: metres/second
                AtmosphericPressure = (long)jsonDTO.Main.Pressure, // default: hectoPascal
                Humidity = jsonDTO.Main.Humidity // %
            };
        }

        public static List<WeatherStateDTO> ParseList(SQLiteDataReader reader)
        {
            List<WeatherStateDTO> result = new List<WeatherStateDTO>();
            while (reader.Read())
            {
                result.Add(ParseCurrent(reader));
            }
            return result;
        }

        public static WeatherStateDTO ParseCurrent(SQLiteDataReader reader)
        {
            return new WeatherStateDTO()
            {
                CityID = reader.GetInt64(0),
                Moment = reader.GetDateTime(1),
                Description = reader.GetString(2),
                Temperature = reader.GetDouble(3),
                Cloudiness = reader.GetDouble(4),
                WindSpeed = reader.GetDouble(5),
                AtmosphericPressure = reader.GetInt64(6),
                Humidity = reader.GetInt64(7)
            };
        }

        public override string ToSQL
            => $"{CityID}, {Moment.ToSqlDateTime()}, '{Description}', {Temperature}, {Cloudiness}, {WindSpeed}, {AtmosphericPressure}, {Humidity}";
    }
}
