using CommonCode;
using DataLayer;
using Jobs.GoogleFlights.DTOs;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Jobs.GoogleFlights
{
    public enum GoogleFlightsTables
    {
        Routes,
        Prices
    }

    public class GoogleFlightsDL : BaseDL<GoogleFlightsTables>
    {
        private SQLiteConnection _connection;
        protected override SQLiteConnection Connection
            => GetConnection(ref _connection, Const.DATABASE_GOOGLE_FLIGHTS);

        protected override void InitializeTableOverview()
        {
            AddTable(GoogleFlightsTables.Routes, "ID INTEGER, FromCity TEXT, ToCity TEXT, FlightDate DATE, Passengers INTEGER, LastUpdate DATETIME, URL TEXT", "ID, FromCity, ToCity, FlightDate, Passengers, LastUpdate, URL");
            AddTable(GoogleFlightsTables.Prices, "RouteID INTEGER, Moment DATETIME, FromAirport TEXT, ToAirport TEXT, TakeOffTime TIME, LandingTime TIME, FlightDuration TIME, Company TEXT, Layovers TEXT, Price INTEGER, Currency TEXT", "RouteID, Moment, FromAirport, ToAirport, TakeOffTime, LandingTime, FlightDuration, Company, Layovers, Price, Currency");
        }

        private string SelectRoutesToUpdateWhereContents(int minimumUpdateIntervalInHours)
            => $"LastUpdate < DATETIME('now', '-{minimumUpdateIntervalInHours} hours')";

        public bool ThereAreRoutesToUpdate(int minimumUpdateIntervalInHours)
        {
            return (long)ExecuteSQLScalar($"SELECT EXISTS(SELECT * FROM Routes WHERE {SelectRoutesToUpdateWhereContents(minimumUpdateIntervalInHours)})") == 1;
        }

        public List<RouteDTO> SelectRoutesToUpdate(int minimumUpdateIntervalInHours, bool onlyToUpdate = true)
        {
            return RouteDTO.ParseList(Select(GoogleFlightsTables.Routes, onlyToUpdate ? SelectRoutesToUpdateWhereContents(minimumUpdateIntervalInHours) : "", "LastUpdate DESC"));
        }

        public void InsertNewData(RouteDTO route, List<PriceDTO> prices)
        {
            ExecuteSQL($"UPDATE {Tables[GoogleFlightsTables.Routes].Name} SET LastUpdate = {route.LastUpdate.ToSqlDateTime()} WHERE ID = {route.ID}");
            foreach (PriceDTO price in prices)
                InsertData(GoogleFlightsTables.Prices, price.ToSQL);
        }
    }
}
