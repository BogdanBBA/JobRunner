using CommonCode;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Jobs.GoogleFlights.DTOs
{
    public class RouteDTO : BaseDTO
    {
        public long ID { get; set; }
        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public DateTime FlightDate { get; set; }
        public long Passengers { get; set; }
        public DateTime LastUpdate { get; set; }
        public string URL { get; set; }

        public static List<RouteDTO> ParseList(SQLiteDataReader reader)
        {
            List<RouteDTO> result = new List<RouteDTO>();
            while (reader.Read())
            {
                result.Add(ParseCurrent(reader));
            }
            return result;
        }

        public static RouteDTO ParseCurrent(SQLiteDataReader reader)
        {
            return new RouteDTO()
            {
                ID = reader.GetInt64(0),
                FromCity = reader.GetString(1),
                ToCity = reader.GetString(2),
                FlightDate = reader.GetDateTime(3),
                Passengers = reader.GetInt64(4),
                LastUpdate = reader.GetDateTime(5),
                URL = reader.GetString(6)
            };
        }

        public override string ToSQL
            => $"{ID}, '{FromCity}', '{ToCity}', {FlightDate.ToSqlDate()}, {Passengers}, {LastUpdate.ToSqlDateTime()}, '{URL}'";
    }
}
