using CommonCode;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Jobs.OpenWeather.DTOs
{
    public class CityDTO : BaseDTO
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public long Population { get; set; }
        public long Altitude { get; set; }
        public double XLongitude { get; set; }
        public double YLatitude { get; set; }
        public DateTime LastUpdate { get; set; }

        public static List<CityDTO> ParseList(SQLiteDataReader reader)
        {
            List<CityDTO> result = new List<CityDTO>();
            while (reader.Read())
            {
                result.Add(ParseCurrent(reader));
            }
            return result;
        }

        public static CityDTO ParseCurrent(SQLiteDataReader reader)
        {
            return new CityDTO()
            {
                ID = reader.GetInt64(0),
                Name = reader.GetString(1),
                Population = reader.GetInt64(2),
                Altitude = reader.GetInt64(3),
                XLongitude = reader.GetDouble(4),
                YLatitude = reader.GetDouble(5),
                LastUpdate = reader.GetDateTime(6)
            };
        }

        public override string ToSQL
            => $"{ID}, '{Name}', {Population}, {Altitude}, {XLongitude}, {YLatitude}, {LastUpdate.ToSqlDateTime()}";
    }
}
