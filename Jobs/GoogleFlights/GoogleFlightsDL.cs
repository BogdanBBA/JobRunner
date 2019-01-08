using DataLayer;
using Jobs.GoogleFlights.DTOs;
using System.Collections.Generic;
using System.Data.SQLite;

namespace Jobs.GoogleFlights
{
    public enum GoogleFligtTables
    {
        Recordings,
        Flights
    }

    public class GoogleFlightsDL : BaseDL<GoogleFligtTables>
    {
        private SQLiteConnection _connection;
        protected override SQLiteConnection Connection
            => GetConnection(ref _connection, Const.DATABASE_GOOGLE_FLIGHTS);

        protected override void InitializeTableOverview()
        {
            AddTable(GoogleFligtTables.Recordings, "ID INTEGER, FromCity TEXT, ToCity TEXT, FlightDate DATE, Moment DATETIME, URL TEXT", "ID, FromCity, ToCity, FlightDate, Moment, URL");
            AddTable(GoogleFligtTables.Flights, "X INTEGER", "X");
        }

        public void InsertNewData(RecordingDTO recording, List<FlightDTO> flights)
        {
            flights.ForEach(flight => flight.RecordingID = recording.ID);
            
        }
    }
}
