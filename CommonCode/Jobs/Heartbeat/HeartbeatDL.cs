using CommonCode.DataLayer;
using CommonCode.Utils;
using System;
using System.Data.SQLite;

namespace Jobs.Heartbeat
{
    public enum HeartbeatTables
    {
        Dates
    }

    public class HeartbeatDL : BaseDL<HeartbeatTables>
    {
        private SQLiteConnection _connection;
        protected override SQLiteConnection Connection
            => GetConnection(ref _connection, Const.DATABASE_HEARTBEAT);

        protected override void InitializeTableOverview()
        {
            AddTable(HeartbeatTables.Dates, "Moment DATE", "Moment");
        }

        public bool DateExists(DateTime moment)
        {
            return (long)ExecuteSQLScalar($"SELECT EXISTS(SELECT Moment FROM Dates WHERE Moment = {DateTime.Now.ToSqlDate()})") == 1;
        }
    }
}
