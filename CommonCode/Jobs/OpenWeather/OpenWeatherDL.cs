using CommonCode.DataLayer;
using CommonCode.Utils;
using Jobs.OpenWeather.DTOs;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Jobs.OpenWeather
{
    public enum OpenWeatherTables
    {
        Cities,
        WeatherStates
    }

    public class OpenWeatherDL : BaseDL<OpenWeatherTables>
    {
        private SQLiteConnection _connection;
        protected override SQLiteConnection Connection
            => GetConnection(ref _connection, Const.DATABASE_OPEN_WEATHER);

        protected override void InitializeTableOverview()
        {
            AddTable(OpenWeatherTables.Cities, "ID INTEGER, Name TEXT, Population INTEGER, Altitude INTEGER, XLongitude REAL, YLatitude REAL, LastUpdate DATETIME", "ID, Name, Population, Altitude, XLongitude, YLatitude, LastUpdate");
            AddTable(OpenWeatherTables.WeatherStates, "CityID INTEGER, Moment DATETIME, Description TEXT, Temperature REAL, Cloudiness REAL, WindSpeed REAL, AtmosphericPressure INTEGER, Humidity INTEGER", "CityID, Moment, Description, Temperature, Cloudiness, WindSpeed, AtmosphericPressure, Humidity");
        }

        private string Oldest20Query(int minAgeInHours, int rowLimit)
            => $"SELECT {Tables[OpenWeatherTables.Cities].ColumnList} FROM Cities WHERE DATETIME() > DATETIME(LastUpdate, '+{minAgeInHours} hour') ORDER BY LastUpdate ASC, Population DESC LIMIT {rowLimit}";

        public bool ThereAreCitiesToUpdateNow(int minAgeInHours, int rowLimit)
            => (long)ExecuteSQLScalar($"SELECT EXISTS({Oldest20Query(minAgeInHours, rowLimit)})") == 1;

        public List<CityDTO> SelectOldest20Cities(int minAgeInHours, int rowLimit)
        {
            return CityDTO.ParseList(ExecuteSQLReader(Oldest20Query(minAgeInHours, rowLimit)));
        }

        public void AddWeatherStateAndUpdateCity(WeatherStateDTO weather)
        {
            ExecuteSQL($"UPDATE Cities SET LastUpdate = {weather.Moment.ToSqlDateTime()} WHERE ID = {weather.CityID}");
            InsertData(OpenWeatherTables.WeatherStates, weather.ToSQL);
        }
    }
}