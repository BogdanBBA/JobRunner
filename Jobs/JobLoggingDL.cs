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
    }
}
