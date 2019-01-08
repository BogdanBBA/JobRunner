using DataLayer;
using System.Data.SQLite;

namespace Jobs
{
    public enum JobLoggingTables
    {
        Runs
    }

    public class JobLoggingDL : BaseDL<JobLoggingTables>
    {
        private SQLiteConnection _connection;
        protected override SQLiteConnection Connection 
            => GetConnection(ref _connection, Const.DATABASE_JOB_LOGGING);
        
        protected override void InitializeTableOverview()
        {
            AddTable(JobLoggingTables.Runs, "JobID INTEGER, Moment DATETIME, ResultCode INTEGER, ErrorDescription TEXT", "JobID, Moment, ResultCode, ErrorDescription");
        }

        private string Last24HoursQuery
            => "DATETIME('now', '-1 day') < Moment";

        private string ErrorLogsInLast24HWhereContents
            => $"{Last24HoursQuery} AND ResultCode != 0 AND ResultCode != 1";

        public int SelectLast24HErrorLogCount()
            => (int)ExecuteSQLScalar($"SELECT COUNT(*) FROM Runs WHERE {ErrorLogsInLast24HWhereContents}"); // ORDER BY Moment DESC

        public int SelectLast24HLogCount()
            => (int)ExecuteSQLScalar($"SELECT COUNT(*) FROM Runs WHERE {Last24HoursQuery}");
    }
}
